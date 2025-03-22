using System.ComponentModel.DataAnnotations;

namespace TeamHub.Application.Models;

/// <summary>
/// Represents a user with profile, login, and role information.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Gets or sets the user ID (optional).
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 15 characters.")]
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets the username used for login.
    /// </summary>
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 10 characters.")]
    [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6-20 characters and must contain at least one" +
                                                        " non-alphanumeric character, one digit, and one uppercase letter.")]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the optional virtual path to the user's profile picture.
    /// </summary>
    public string? VirtualPath { get; set; }

    /// <summary>
    /// Gets or sets the roles assigned to the user.
    /// </summary>
    [Required(ErrorMessage = "At least one role is required.")]
    [MinLength(1, ErrorMessage = "User must have at least one role assigned.")]
    public List<string> Roles { get; set; } = new List<string>();
}
