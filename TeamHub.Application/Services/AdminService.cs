using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Application.Result;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data;

namespace TeamHub.Application.Services;

/// <summary>
/// Provides admin-related operations.
/// </summary>
public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminService> _logger;

    public AdminService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<AdminService> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc cref="IAdminService.GetAllUsers"/>
    public async Task<Result<List<UserModel>>> GetAllUsers()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = roles.ToList()
                });
            }

            _logger.LogInformation("Successfully fetched {Count} users.", userList.Count);
            return Result<List<UserModel>>.Ok(userList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching users.");
            return Result<List<UserModel>>.Fail("An unexpected error occurred while fetching users.");
        }
    }

    /// <inheritdoc cref="IAdminService.GetUserById"/>
    public async Task<Result<UserModel>> GetUserById(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return Result<UserModel>.Fail("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Result<UserModel>.Ok(new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles.ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching user with ID: {UserId}", userId);
            return Result<UserModel>.Fail("An unexpected error occurred while fetching the user.");
        }
    }

    /// <inheritdoc cref="IAdminService.CreateEmployee"/>
    public async Task<Result<UserModel>> CreateEmployee(UserModel model)
    {
        try
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                _logger.LogWarning("User already exists with email: {Email}", model.Email);
                return Result<UserModel>.Fail("User email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));

                _logger.LogWarning("Failed to create employee for email: {Email}. Errors: {Errors}",
                    model.Email, errorMessages);

                return Result<UserModel>.Fail(errorMessages);
            }

            await _userManager.AddToRoleAsync(user, "Employee");

            return Result<UserModel>.Ok(new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = new List<string> { "Employee" }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating employee with email: {Email}", model.Email);
            return Result<UserModel>.Fail("Unexpected error occurred while creating employee.");
        }
    }

    /// <inheritdoc cref="IAdminService.UpdateUser"/>
    public async Task<Result<UserModel>> UpdateUser(string userId, UserModel model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for update with ID: {UserId}", userId);
                return Result<UserModel>.Fail("User not found.");
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Username;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));

                _logger.LogWarning("Failed to update user with ID: {UserId}. Errors: {Errors}",
                    userId, errorMessages);

                return Result<UserModel>.Fail(errorMessages);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Result<UserModel>.Ok(new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles.ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {UserId}", userId);
            return Result<UserModel>.Fail("Unexpected error occurred while updating user.");
        }
    }

    /// <inheritdoc cref="IAdminService.DeleteUser"/>
    public async Task<Result<bool>> DeleteUser(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for deletion with ID: {UserId}", userId);
                return Result<bool>.Fail("User not found.");
            }

            // Check if user is assigned to any project
            bool isInProjects = await _context.ProjectEmployees.AnyAsync(pe => pe.EmployeeId == userId);

            // Check if user is assigned to any task
            bool isInTasks = await _context.Tasks.AnyAsync(t => t.AssignedToId == userId);

            if (isInProjects || isInTasks)
            {
                _logger.LogWarning("Cannot delete user with ID {UserId} because they are assigned to projects or tasks.", userId);
                return Result<bool>.Fail("Cannot delete user who is assigned to projects or tasks.");
            }

            await _userManager.DeleteAsync(user);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", userId);
            return Result<bool>.Fail("Unexpected error occurred while deleting user.");
        }
    }
}
