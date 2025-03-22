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
    [Required(ErrorMessage = "Project name is required.")]
    [StringLength(15, ErrorMessage = "Project name cannot exceed 15 characters.")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    [StringLength(20, ErrorMessage = "Description cannot exceed 20 characters.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the list of tasks associated with the project.
    /// </summary>
    public List<TaskItem> Tasks { get; set; } = new();
}