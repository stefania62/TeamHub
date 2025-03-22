using TeamHub.Application.Models;

namespace TeamHub.Application.Interfaces;

/// <summary>
/// Provides operations related to user profile management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves the profile information of a user by their ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="UserModel"/> containing the user's profile details.</returns>
    Task<UserModel> GetProfile(string userId);

    /// <summary>
    /// Updates the profile information of a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="model">The updated user profile data.</param>
    /// <returns>A boolean indicating whether the update was successful.</returns>
    Task<bool> UpdateProfile(string userId, UserModel model);
}
