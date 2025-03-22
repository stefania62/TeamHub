using TeamHub.Application.Models;
using TeamHub.Application.Result;

namespace TeamHub.Application.Interfaces;
public interface IProjectService
{
    Task<Result<List<ProjectModel>>> GetProjects(string userId, List<string> userRoles);
    Task<Result<ProjectModel>> CreateProject(ProjectModel model);
    Task<Result<ProjectModel>> UpdateProject(int projectId, ProjectModel model);
    Task<Result<bool>> DeleteProject(int projectId);
    Task<Result<bool>> AssignEmployeeToProject(int projectId, string employeeId);
    Task<Result<bool>> RemoveEmployeeFromProject(int projectId, string employeeId);
}
