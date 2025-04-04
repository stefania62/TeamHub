﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Application.Result;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data.Context;

namespace TeamHub.Application.Services;

/// <summary>
/// Provides project-related operations.
/// </summary>
public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProjectService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectService"/>.
    /// </summary>
    /// <param name="context">Database context for accessing data.</param>
    /// <param name="logger">Handles logging.</param>
    public ProjectService(ApplicationDbContext context, ILogger<ProjectService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc cref="IProjectService.GetProjects"/>
    public async Task<Result<List<ProjectModel>>> GetProjects(string userId, List<string> userRoles)
    {
        try
        {
            List<Project> projects;

            if (userRoles.Contains(nameof(UserRole.Administrator)))
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

            var result = projects.Select(project => new ProjectModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
            }).ToList();

            return Result<List<ProjectModel>>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting projects for user {UserId}", userId);
            return Result<List<ProjectModel>>.Fail("Unexpected error occurred while fetching projects.");
        }
    }

    /// <inheritdoc cref="IProjectService.CreateProject"/>
    public async Task<Result<ProjectModel>> CreateProject(ProjectModel model)
    {
        try
        {
            var project = new Project
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Result<ProjectModel>.Ok(new ProjectModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating project: {ProjectTitle}", model.Title);
            return Result<ProjectModel>.Fail("Unexpected error occurred while creating project.");
        }
    }

    /// <inheritdoc cref="IProjectService.UpdateProject"/>
    public async Task<Result<ProjectModel>> UpdateProject(int projectId, ProjectModel model)
    {
        try
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                _logger.LogWarning("Project with ID {ProjectId} not found for update.", projectId);
                return Result<ProjectModel>.Fail("Project not found.");
            }

            project.Title = model.Title;
            project.Description = model.Description;
            project.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated project ID {ProjectId}", project.Id);

            return Result<ProjectModel>.Ok(new ProjectModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating project ID {ProjectId}", projectId);
            return Result<ProjectModel>.Fail("Unexpected error occurred while updating project.");
        }
    }

    /// <inheritdoc cref="IProjectService.AssignEmployeeToProject"/>
    public async Task<Result<bool>> AssignEmployeeToProject(int projectId, string employeeId)
    {
        try
        {
            var project = await _context.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            var employee = await _context.Users.FindAsync(employeeId);

            if (project == null || employee == null)
            {
                _logger.LogWarning("Project or employee not found for assignment. ProjectID: {ProjectId}, EmployeeID: {EmployeeId}", projectId, employeeId);
                return Result<bool>.Fail("Project or employee not found.");
            }

            bool isAlreadyAssigned = await _context.ProjectEmployees
                .AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (isAlreadyAssigned)
            {
                _logger.LogInformation("Employee ID {EmployeeId} is already assigned to project ID {ProjectId}", employeeId, projectId);
                return Result<bool>.Fail("Employee is already assigned to this project.");
            }

            _context.ProjectEmployees.Add(new ProjectEmployee
            {
                ProjectId = projectId,
                EmployeeId = employeeId,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            _logger.LogInformation("Assigned employee ID {EmployeeId} to project ID {ProjectId}", employeeId, projectId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning employee ID {EmployeeId} to project ID {ProjectId}", employeeId, projectId);
            return Result<bool>.Fail("Unexpected error occurred while assigning employee to project.");
        }
    }

    /// <inheritdoc cref="IProjectService.RemoveEmployeeFromProject"/>
    public async Task<Result<bool>> RemoveEmployeeFromProject(int projectId, string employeeId)
    {
        try
        {
            var projectEmployee = await _context.ProjectEmployees
                .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (projectEmployee == null)
            {
                _logger.LogWarning("Employee ID {EmployeeId} not found in project ID {ProjectId}", employeeId, projectId);
                return Result<bool>.Fail("Employee is not part of this project.");
            }

            _context.ProjectEmployees.Remove(projectEmployee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Removed employee ID {EmployeeId} from project ID {ProjectId}", employeeId, projectId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing employee ID {EmployeeId} from project ID {ProjectId}", employeeId, projectId);
            return Result<bool>.Fail("Unexpected error occurred while removing employee from project.");
        }
    }

    /// <inheritdoc cref="IProjectService.DeleteProject"/>
    public async Task<Result<bool>> DeleteProject(int projectId)
    {
        try
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                _logger.LogWarning("Project ID {ProjectId} not found for deletion.", projectId);
                return Result<bool>.Fail("Project not found.");
            }

            if (project.Tasks.Any(t => !t.IsCompleted))
            {
                _logger.LogWarning("Cannot delete project ID {ProjectId} with open tasks.", projectId);
                return Result<bool>.Fail("Cannot delete project with open tasks.");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted project ID {ProjectId}", projectId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting project ID {ProjectId}", projectId);
            return Result<bool>.Fail("Unexpected error occurred while deleting project.");
        }
    }
}
