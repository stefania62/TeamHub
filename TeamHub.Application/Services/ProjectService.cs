using Microsoft.EntityFrameworkCore;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data;

namespace TeamHub.Application.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectModel>> GetProjects(string userId, List<string> userRoles)
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

    public async Task<ProjectModel> CreateProject(ProjectModel model)
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

    public async Task<ProjectModel> UpdateProject(int projectId, ProjectModel model)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return null;

        project.Name = model.Name;
        project.Description = model.Description;
        await _context.SaveChangesAsync();

        return new ProjectModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description
        };
    }

    public async Task<bool> DeleteProject(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null || project.Tasks.Any(t => !t.IsCompleted))
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignEmployeeToProject(int projectId, string employeeId)
    {
        var project = await _context.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
        var employee = await _context.Users.FindAsync(employeeId);

        if (project == null || employee == null) return false;

        bool isAlreadyAssigned = await _context.ProjectEmployees
            .AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

        if (isAlreadyAssigned) return false;

        _context.ProjectEmployees.Add(new ProjectEmployee
        {
            ProjectId = projectId,
            EmployeeId = employeeId
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveEmployeeFromProject(int projectId, string employeeId)
    {
        var projectEmployee = await _context.ProjectEmployees
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

        if (projectEmployee == null) return false;

        _context.ProjectEmployees.Remove(projectEmployee);
        await _context.SaveChangesAsync();
        return true;
    }
}

