using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamHub.Application.Services;
using TeamHub.Domain.Entities;
using TeamHub.Application.Models;
using TeamHub.Infrastructure.Data.Settings;
using System.Security.Claims;
using System.Reflection;

namespace TeamHub.Tests.Services
{
    /// <summary>
    /// Unit tests for AuthService focusing on critical login scenarios.
    /// </summary>
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            _loggerMock = new Mock<ILogger<AuthService>>();

            var jwtSettings = Options.Create(new JwtSettings
            {
                Secret = "ThisIsASecretKeyThatIsLongEnoughToWork123!", // must be >= 32 chars
                Issuer = "TeamHub",
                Audience = "TeamHubUsers",
                ExpiryMinutes = 30
            });

            _authService = new AuthService(
                _userManagerMock.Object,
                jwtSettings,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Should return null if the model is null or missing required fields.
        /// </summary>
        [Fact]
        public async Task AuthenticateUser_ShouldReturnNull_WhenModelIsInvalid()
        {
            // Act
            var result = await _authService.AuthenticateUser(new LoginModel { Email = "", Password = "" });

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Should return null if the user doesn't exist.
        /// </summary>
        [Fact]
        public async Task AuthenticateUser_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _authService.AuthenticateUser(new LoginModel
            {
                Email = "nonexistent@example.com",
                Password = "Secret123!"
            });

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Should return null if the password check fails.
        /// </summary>
        [Fact]
        public async Task AuthenticateUser_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@example.com" };
            _userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "wrongpassword")).ReturnsAsync(false);

            // Act
            var result = await _authService.AuthenticateUser(new LoginModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            });

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Should return a valid JWT token if credentials are correct.
        /// </summary>
        [Fact]
        public async Task AuthenticateUser_ShouldReturnToken_WhenValidLogin()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "user123",
                Email = "valid@example.com",
                FullName = "Valid User"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "correct")).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Employee" });

            // Act
            var result = await _authService.AuthenticateUser(new LoginModel
            {
                Email = "valid@example.com",
                Password = "correct"
            });

            // Assert
            Assert.False(string.IsNullOrEmpty(result)); // Token should not be null or empty
            Assert.Contains(".", result);
        }

        /// <summary>
        /// Should throw ArgumentException if JWT secret is too short.
        /// </summary>
        [Fact]
        public void GenerateJwtToken_ShouldThrow_WhenSecretTooShort()
        {
            // Arrange
            var shortSecretOptions = Options.Create(new JwtSettings
            {
                Secret = "short",
                Issuer = "TeamHub",
                Audience = "TeamHubUsers",
                ExpiryMinutes = 30
            });

            var service = new AuthService(
                _userManagerMock.Object,
                shortSecretOptions,
                _loggerMock.Object
            );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@example.com")
            };

            // Act
            var methodInfo = typeof(AuthService)
                .GetMethod("GenerateJwtToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
            {
                methodInfo.Invoke(service, new object[] { claims });
            });

            // Unwrap and assert the real inner exception
            Assert.IsType<ArgumentException>(exception.InnerException);
            Assert.Equal("JWT secret key must be at least 32 characters long.", exception.InnerException.Message);
        }
    }
}
