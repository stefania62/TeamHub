using TeamHub.Application.Models;

namespace TeamHub.Application.Interfaces
{
    /// <summary>
    /// Provides authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using the provided login credentials.
        /// </summary>
        /// <param name="model">The login model containing username/email and password.</param>
        /// <returns>A JWT token if authentication is successful; otherwise, null or an error.</returns>
        Task<string> AuthenticateUser(LoginModel model);
    }
}