using Microsoft.AspNetCore.Identity;

namespace TeamHub.Domain.Entities;

/// <summary>
/// Custom user model for authentication and domain-related data.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the virtual path to the user's profile picture.
    /// </summary>
    public string? ImageVirtualPath { get; set; }

    /// <summary>
    /// Gets or sets the collection of project associations for the user.
    /// </summary>
    public ICollection<ProjectEmployee> Projects { get; set; } = new List<ProjectEmployee>();

    /// <summary>
    /// Gets or sets the collection of tasks assigned to the user.
    /// </summary>
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    /// <summary>
    /// The timestamp when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The timestamp when the user was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}