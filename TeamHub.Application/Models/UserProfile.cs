using System.ComponentModel.DataAnnotations;

namespace TeamHub.Application.Models;

/// <summary>
/// User profile model.
/// </summary>
public class UserProfile
{
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name should be between 3 and 100 characters.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Full Name cannot be empty spaces.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 10 characters.")]
    [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    public string Username { get; set; }

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be at least 6 characters.")]
    public string? Password { get; set; }

    public string? VirtualPath { get; set; }

    [Required(ErrorMessage = "At least one role is required.")]
    [MinLength(1, ErrorMessage = "User must have at least one role assigned.")]
    public List<string> Roles { get; set; } = new List<string>();
}