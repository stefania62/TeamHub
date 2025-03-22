using TeamHub.Application.Models;
using TeamHub.Application.Result;

namespace TeamHub.Application.Interfaces;

/// <summary>
/// Defines contract for project-related operations.
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Retrieves all projects accessible to a user based on their roles.
    /// </summary>
    /// <param name="userId">The ID of the requesting user.</param>
    /// <param name="userRoles">The roles assigned to the user.</param>
    /// <returns>A result containing the list of accessible projects.</returns>
    Task<Result<List<ProjectModel>>> GetProjects(string userId, List<string> userRoles);

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="model">The project details.</param>
    /// <returns>A result containing the created project model.</returns>
    Task<Result<ProjectModel>> CreateProject(ProjectModel model);

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="projectId">The ID of the project to update.</param>
    /// <param name="model">The updated project details.</param>
    /// <returns>A result indicating success or failure, and the updated project model.</returns>
    Task<Result<ProjectModel>> UpdateProject(int projectId, ProjectModel model);

    /// <summary>
    /// Assigns an employee to a project.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="employeeId">The ID of the employee to assign.</param>
    /// <returns>A result indicating whether the assignment was successful.</returns>
    Task<Result<bool>> AssignEmployeeToProject(int projectId, string employeeId);

    /// <summary>
    /// Removes an employee from a project.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="employeeId">The ID of the employee to remove.</param>
    /// <returns>A result indicating whether the removal was successful.</returns>
    Task<Result<bool>> RemoveEmployeeFromProject(int projectId, string employeeId);

    /// <summary>
    /// Deletes a project.
    /// </summary>
    /// <param name="projectId">The ID of the project to delete.</param>
    /// <returns>A result indicating whether the deletion was successful.</returns>
    Task<Result<bool>> DeleteProject(int projectId);
}
