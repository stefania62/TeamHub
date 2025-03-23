using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Application.Result;
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
    public async Task<Result<UserModel>> GetProfile(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found while fetching profile.", userId);
                return Result<UserModel>.Fail("User not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return Result<UserModel>.Ok(new UserModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Roles = userRoles.ToList(),
                VirtualPath = user.ImageVirtualPath
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching profile for user {UserId}.", userId);
            return Result<UserModel>.Fail("Unexpected error occurred while fetching profile.");
        }
    }

    /// <inheritdoc cref="IUserService.UpdateProfile"/>
    public async Task<Result<bool>> UpdateProfile(string userId, UserModel model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for profile update.", userId);
                return Result<bool>.Fail("User not found.");
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Username;

            // Save profile picture if provided
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                var fileName = $"{userId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(model.ProfilePicture.FileName)}";
                var folderPath = Path.Combine("wwwroot", "uploads", "profile-pictures");
                var filePath = Path.Combine(folderPath, fileName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ImageVirtualPath = $"/uploads/profile-pictures/{fileName}";
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!passwordResult.Succeeded)
                {
                    var passwordErrors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Password update failed for user {UserId}: {Errors}", userId, passwordErrors);
                    return Result<bool>.Fail($"Password update failed: {passwordErrors}");
                }

                _logger.LogInformation("Password updated successfully for user {UserId}.", userId);
            }


            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to update profile for user {UserId}. Errors: {Errors}", userId, errors);
                return Result<bool>.Fail(errors);
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating profile for user {UserId}.", userId);
            return Result<bool>.Fail("Unexpected error occurred while updating profile.");
        }
    }

}
