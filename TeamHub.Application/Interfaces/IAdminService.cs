using TeamHub.Application.Models;
using TeamHub.Application.Result;

namespace TeamHub.Application.Interfaces;

/// <summary>
/// Defines admin-related operations.
/// </summary>
public interface IAdminService
{
    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <returns>A result containing a list of user models.</returns>
    Task<Result<List<UserModel>>> GetAllUsers(string? userId);

    /// <summary>
    /// Retrieves a specific user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A result containing the user model if found.</returns>
    Task<Result<UserModel>> GetUserById(string userId);

    /// <summary>
    /// Creates a new employee user.
    /// </summary>
    /// <param name="model">The user model containing employee details.</param>
    /// <returns>A result containing the created user model.</returns>
    Task<Result<UserModel>> CreateEmployee(UserModel model);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="model">The user profile with updated data.</param>
    /// <returns>A result containing the updated user model.</returns>
    Task<Result<UserModel>> UpdateUser(string userId, UserModel model);

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result<bool>> DeleteUser(string userId);
}