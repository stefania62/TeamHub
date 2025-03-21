using System.ComponentModel.DataAnnotations;
using TeamHub.Domain.Entities;

namespace TeamHub.Application.Models;

/// <summary>
/// Represents a project model.
/// </summary>
public class ProjectModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Project name is required.")]
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();
}

