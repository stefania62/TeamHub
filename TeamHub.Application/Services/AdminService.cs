using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;

namespace TeamHub.Application.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AdminService> _logger;

    public AdminService(UserManager<ApplicationUser> userManager, ILogger<AdminService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<UserModel>> GetAllUsers()
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
            return userList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching users.");
            throw;
        }
    }

    public async Task<UserModel> GetUserById(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles.ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching user with ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<UserModel> CreateEmployee(UserModel model)
    {
        try
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                _logger.LogWarning("User already exists with email: {Email}", model.Email);
                return null;
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to create employee for email: {Email}. Errors: {Errors}",
                    model.Email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return null;
            }

            await _userManager.AddToRoleAsync(user, "Employee");

            return new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = new List<string> { "Employee" }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating employee with email: {Email}", model.Email);
            throw;
        }
    }
    public async Task<UserModel> UpdateUser(string userId, UserProfile model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for update with ID: {UserId}", userId);
                return null;
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Username;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to update user with ID: {UserId}. Errors: {Errors}",
                    userId,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles.ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> DeleteUser(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for deletion with ID: {UserId}", userId);
                return false;
            }

            await _userManager.DeleteAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", userId);
            throw;
        }
    }
}
