using TeamHub.API.Models;

namespace TeamHub.API.Entities
{
    /// <summary>
    /// Represents a project.
    /// </summary>
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<ProjectEmployee> Employees { get; set; } = new List<ProjectEmployee>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
