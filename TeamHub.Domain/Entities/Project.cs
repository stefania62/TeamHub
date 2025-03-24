namespace TeamHub.Domain.Entities;

/// <summary>
/// Represents a project.
/// </summary>
public class Project : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the collection of employees assigned to this project.
    /// </summary>
    public ICollection<ProjectEmployee> Employees { get; set; } = new List<ProjectEmployee>();

    /// <summary>
    /// Gets or sets the collection of tasks associated with this project.
    /// </summary>
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}