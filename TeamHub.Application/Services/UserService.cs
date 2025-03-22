using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;

namespace TeamHub.Application.Services;

/// <summary>
/// Provides functionality for managing user profiles.
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    /// <inheritdoc cref="IUserService.GetProfile"/>
    public async Task<UserModel> GetProfile(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found while fetching profile.", userId);
                return null;
            }

            return new UserModel
            {
                FullName = user.FullName,
                Email = user.Email,
                VirtualPath = user.ProfilePicture
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching profile for user {UserId}.", userId);
            throw;
        }
    }

    /// <inheritdoc cref="IUserService.UpdateProfile"/>
    public async Task<bool> UpdateProfile(string userId, UserModel model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for profile update.", userId);
                return false;
            }

            user.FullName = model.FullName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                _logger.LogWarning("Failed to update profile for user {UserId}. Errors: {Errors}",
                    userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating profile for user {UserId}.", userId);
            throw;
        }
    }
}
