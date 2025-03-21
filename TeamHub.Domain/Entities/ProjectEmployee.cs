namespace TeamHub.API.Entities
{
    /// <summary>
    /// Represents the relationship between Employees and Projects.
    /// </summary>
    public class ProjectEmployee
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public string EmployeeId { get; set; } = string.Empty;
        public ApplicationUser Employee { get; set; } = null!;
    }
}
