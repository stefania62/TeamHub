using System.ComponentModel.DataAnnotations;
using TeamHub.Domain.Entities;

namespace TeamHub.Application.Models;

/// <summary>
/// Represents a project model.
/// </summary>
public class ProjectModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(20, ErrorMessage = "Project title cannot exceed 20 characters.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
    public string? Description { get; set; }

}