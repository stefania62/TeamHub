using System.ComponentModel.DataAnnotations;

namespace TeamHub.Application.Models;

/// <summary>
/// Represents a task within a project.
/// </summary>
public class TaskModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    [Required(ErrorMessage = "Task title is required.")]
    [StringLength(20, ErrorMessage = "Task title cannot exceed 20 characters.")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the ID of the project this task belongs to.
    /// </summary>
    [Required(ErrorMessage = "Project ID is required.")]
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string ProjectTitle { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user assigned to the task.
    /// </summary>
    public string? AssignedUserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the user assigned to the task.
    /// </summary>
    public string? AssignedUserName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task is assigned to current user.
    /// </summary>
    public bool AssignedToCurrentUser { get; set; }
}