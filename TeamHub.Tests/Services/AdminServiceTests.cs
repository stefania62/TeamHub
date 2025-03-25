using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TeamHub.Application.Services;
using TeamHub.Domain.Entities;
using TeamHub.Application.Models;
using TeamHub.Infrastructure.Data.Context;
using TeamHub.Application.Interfaces;

namespace TeamHub.Tests.Services
{
    /// <summary>
    /// Unit tests for AdminService covering core user management operations.
    /// </summary>
    public class AdminServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly ILogger<AdminService> _logger;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            // Mock UserManager
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            // Setup in-memory EF Core DbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AdminService_" + Guid.NewGuid())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            _eventPublisherMock = new Mock<IEventPublisher>();
            _logger = new LoggerFactory().CreateLogger<AdminService>();

            _adminService = new AdminService(
                _userManagerMock.Object,
                _dbContext,
                _eventPublisherMock.Object,
                _logger
            );
        }

        /// <summary>
        /// Should fail if the email is already registered.
        /// </summary>
        [Fact]
        public async Task CreateEmployee_ShouldFail_IfEmailAlreadyExists()
        {
            var existingUser = new ApplicationUser { Email = "test@example.com" };
            _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com")).ReturnsAsync(existingUser);

            var model = new UserModel
            {
                Email = "test@example.com",
                Username = "existinguser",
                FullName = "Existing User",
                Password = "Secret123!"
            };

            var result = await _adminService.CreateEmployee(model);

            Assert.False(result.Success);
            Assert.Equal("User email already exists.", result.ErrorMessage);
        }

        /// <summary>
        /// Should fail if UserManager.CreateAsync fails during user creation.
        /// </summary>
        [Fact]
        public async Task CreateEmployee_ShouldFail_IfUserCreationFails()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Something went wrong" }));

            var model = new UserModel
            {
                Email = "fail@example.com",
                Username = "failuser",
                FullName = "Fail Case",
                Password = "Secret123!"
            };

            var result = await _adminService.CreateEmployee(model);

            Assert.False(result.Success);
            Assert.Contains("Something went wrong", result.ErrorMessage);
        }

        /// <summary>
        /// Should successfully create a new employee with valid input.
        /// </summary>
        [Fact]
        public async Task CreateEmployee_ShouldSucceed_WhenValid()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), nameof(UserRole.Employee)))
                .ReturnsAsync(IdentityResult.Success);

            _eventPublisherMock.Setup(x => x.PublishUserCreatedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var model = new UserModel
            {
                Email = "new@example.com",
                Username = "newuser",
                FullName = "New User",
                Password = "Secret123!"
            };

            // Act
            var result = await _adminService.CreateEmployee(model);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("new@example.com", result.Data.Email);

            // Verify the event publisher was called once with the expected arguments
            _eventPublisherMock.Verify(x =>
                    x.PublishUserCreatedAsync(
                        It.IsAny<string>(), 
                        "new@example.com",
                        "New User"
                    ),
                Times.Once);
        }

        /// <summary>
        /// Should fail if the user to update is not found.
        /// </summary>
        [Fact]
        public async Task UpdateUser_ShouldFail_IfUserNotFound()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync("not-found")).ReturnsAsync((ApplicationUser)null);

            var model = new UserModel
            {
                FullName = "New Name",
                Email = "new@example.com",
                Username = "newuser"
            };

            var result = await _adminService.UpdateUser("not-found", model);

            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        /// <summary>
        /// Should fail if resetting password fails during update.
        /// </summary>
        [Fact]
        public async Task UpdateUser_ShouldFail_IfPasswordUpdateFails()
        {
            var user = new ApplicationUser { Id = "user789", UserName = "olduser", Email = "old@example.com" };

            _userManagerMock.Setup(x => x.FindByIdAsync("user789")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("reset-token");
            _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "reset-token", "BadPassword"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

            var model = new UserModel
            {
                FullName = "New Name",
                Email = "new@example.com",
                Username = "newuser",
                Password = "BadPassword"
            };

            var result = await _adminService.UpdateUser("user789", model);

            Assert.False(result.Success);
            Assert.Contains("Password update failed", result.ErrorMessage);
        }

        /// <summary>
        /// Should fail if the user to delete is not found.
        /// </summary>
        [Fact]
        public async Task DeleteUser_ShouldFail_IfUserNotFound()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync("unknown")).ReturnsAsync((ApplicationUser)null);

            var result = await _adminService.DeleteUser("unknown");

            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        /// <summary>
        /// Should fail if the user is assigned to any projects or tasks.
        /// </summary>
        [Fact]
        public async Task DeleteUser_ShouldFail_IfUserAssignedToProjectOrTask()
        {
            var user = new ApplicationUser { Id = "user123", Email = "test@test.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync("user123")).ReturnsAsync(user);

            _dbContext.ProjectEmployees.Add(new ProjectEmployee
            {
                ProjectId = 1,
                EmployeeId = "user123"
            });

            _dbContext.Tasks.Add(new TaskItem
            {
                Id = 1,
                AssignedToId = "user123"
            });

            await _dbContext.SaveChangesAsync();

            var result = await _adminService.DeleteUser("user123");

            Assert.False(result.Success);
            Assert.Equal("Cannot delete user who is assigned to projects or tasks.", result.ErrorMessage);
        }

        /// <summary>
        /// Should succeed in deleting user if they are not assigned to anything.
        /// </summary>
        [Fact]
        public async Task DeleteUser_ShouldSucceed_IfUserNotAssigned()
        {
            var user = new ApplicationUser { Id = "user456", Email = "delete@test.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync("user456")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _adminService.DeleteUser("user456");

            Assert.True(result.Success);
            Assert.True(result.Data);
        }
    }
}
