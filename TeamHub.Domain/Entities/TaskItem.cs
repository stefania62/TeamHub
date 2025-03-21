namespace TeamHub.Domain.Entities;

/// <summary>
/// Represents a task within a project.
/// </summary>
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public string AssignedToId { get; set; } = string.Empty;
    public ApplicationUser AssignedTo { get; set; } = null!;
}