using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data;

namespace TeamHub.Application.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(ApplicationDbContext context, ILogger<ProjectService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProjectModel>> GetProjects(string userId, List<string> userRoles)
    {
        try
        {
            List<Project> projects;

            if (userRoles.Contains("Administrator"))
            {
                projects = await _context.Projects.Include(p => p.Tasks).ToListAsync();
            }
            else
            {
                projects = await _context.Projects
                    .Where(p => p.Employees.Any(e => e.EmployeeId == userId))
                    .Include(p => p.Tasks)
                    .ToListAsync();
            }

            return projects.Select(project => new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting projects for user {UserId}", userId);
            throw;
        }
    }

    public async Task<ProjectModel> CreateProject(ProjectModel model)
    {
        try
        {
            var project = new Project
            {
                Name = model.Name,
                Description = model.Description
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating project: {ProjectName}", model.Name);
            throw;
        }
    }

    public async Task<ProjectModel> UpdateProject(int projectId, ProjectModel model)
    {
        try
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                _logger.LogWarning("Project with ID {ProjectId} not found for update.", projectId);
                return null;
            }

            project.Name = model.Name;
            project.Description = model.Description;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated project ID {ProjectId}", project.Id);

            return new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating project ID {ProjectId}", projectId);
            throw;
        }
    }

    public async Task<bool> DeleteProject(int projectId)
    {
        try
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                _logger.LogWarning("Project ID {ProjectId} not found for deletion.", projectId);
                return false;
            }

            if (project.Tasks.Any(t => !t.IsCompleted))
            {
                _logger.LogWarning("Cannot delete project ID {ProjectId} with open tasks.", projectId);
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted project ID {ProjectId}", projectId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting project ID {ProjectId}", projectId);
            throw;
        }
    }

    public async Task<bool> AssignEmployeeToProject(int projectId, string employeeId)
    {
        try
        {
            var project = await _context.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            var employee = await _context.Users.FindAsync(employeeId);

            if (project == null || employee == null)
            {
                _logger.LogWarning("Project or employee not found for assignment. ProjectID: {ProjectId}, EmployeeID: {EmployeeId}", projectId, employeeId);
                return false;
            }

            bool isAlreadyAssigned = await _context.ProjectEmployees
                .AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (isAlreadyAssigned)
            {
                _logger.LogInformation("Employee ID {EmployeeId} is already assigned to project ID {ProjectId}", employeeId, projectId);
                return false;
            }

            _context.ProjectEmployees.Add(new ProjectEmployee
            {
                ProjectId = projectId,
                EmployeeId = employeeId
            });

            await _context.SaveChangesAsync();

            _logger.LogInformation("Assigned employee ID {EmployeeId} to project ID {ProjectId}", employeeId, projectId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning employee ID {EmployeeId} to project ID {ProjectId}", employeeId, projectId);
            throw;
        }
    }

    public async Task<bool> RemoveEmployeeFromProject(int projectId, string employeeId)
    {
        try
        {
            var projectEmployee = await _context.ProjectEmployees
                .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (projectEmployee == null)
            {
                _logger.LogWarning("Employee ID {EmployeeId} not found in project ID {ProjectId}", employeeId, projectId);
                return false;
            }

            _context.ProjectEmployees.Remove(projectEmployee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Removed employee ID {EmployeeId} from project ID {ProjectId}", employeeId, projectId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing employee ID {EmployeeId} from project ID {ProjectId}", employeeId, projectId);
            throw;
        }
    }
}
