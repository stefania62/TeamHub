using System.ComponentModel.DataAnnotations;

namespace TeamHub.Application.Models;

/// <summary>
/// User model.
/// </summary>
public class UserModel
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 characters.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 15 characters.")]
    [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
    public string Password { get; set; }

    public string? VirtualPath { get; set; } // Optional field

    [Required(ErrorMessage = "At least one role is required.")]
    [MinLength(1, ErrorMessage = "User must have at least one role assigned.")]
    public List<string> Roles { get; set; } = new List<string>();
}
