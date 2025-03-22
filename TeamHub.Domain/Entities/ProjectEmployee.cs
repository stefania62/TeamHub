namespace TeamHub.Domain.Entities;

/// <summary>
/// Represents the relationship between employees and projects.
/// </summary>
public class ProjectEmployee
{
    /// <summary>
    /// Gets or sets the ID of the associated project.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project entity associated with this relationship.
    /// </summary>
    public Project Project { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the associated employee.
    /// </summary>
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee entity associated with this relationship.
    /// </summary>
    public ApplicationUser Employee { get; set; } = null!;
}