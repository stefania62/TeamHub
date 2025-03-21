namespace TeamHub.API.Models
{
    /// <summary>
    /// Represents a task within a project.
    /// </summary>
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int ProjectId { get; set; }
        public string AssignedUserId { get; set; }
    }
}