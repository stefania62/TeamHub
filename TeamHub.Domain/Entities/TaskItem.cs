namespace TeamHub.Domain.Entities;

/// <summary>
/// Represents a task within a project.
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional description of the task.
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the foreign key of the associated project.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project to which this task belongs.
    /// </summary>
    public Project Project { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the user assigned to this task.
    /// </summary>
    public string? AssignedToId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user assigned to this task.
    /// </summary>
    public ApplicationUser AssignedTo { get; set; } = null!;
}