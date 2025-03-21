using TeamHub.Application.Models;

namespace TeamHub.Application.Interfaces;
public interface IProjectService
{
    Task<List<ProjectModel>> GetProjects(string userId, List<string> userRoles);
    Task<ProjectModel> CreateProject(ProjectModel model);
    Task<ProjectModel> UpdateProject(int projectId, ProjectModel model);
    Task<bool> DeleteProject(int projectId);
    Task<bool> AssignEmployeeToProject(int projectId, string employeeId);
    Task<bool> RemoveEmployeeFromProject(int projectId, string employeeId);
}
