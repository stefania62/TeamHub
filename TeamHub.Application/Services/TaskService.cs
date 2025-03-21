using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data;

namespace TeamHub.Application.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ApplicationDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaskModel>> GetUserTasks(string userId, List<string> userRoles)
    {
        try
        {
            List<TaskItem> tasks;

            if (userRoles.Contains("Admin"))
            {
                tasks = await _context.Tasks.Include(t => t.Project).ToListAsync();
            }
            else
            {
                tasks = await _context.Tasks
                    .Where(t => _context.ProjectEmployees
                        .Any(pe => pe.ProjectId == t.ProjectId && pe.EmployeeId == userId))
                    .Include(t => t.Project)
                    .ToListAsync();
            }

            return tasks.Select(task => new TaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedToId
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting tasks for user {UserId}", userId);
            throw;
        }
    }

    public async Task<TaskModel> CreateTask(string userId, TaskModel model, List<string> userRoles)
    {
        try
        {
            var project = await _context.Projects
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == model.ProjectId);

            if (project == null || (userRoles.Contains("Employee") && !project.Employees.Any(e => e.EmployeeId == userId)))
            {
                _logger.LogWarning("Unauthorized task creation attempt by user {UserId} for project {ProjectId}", userId, model.ProjectId);
                return null;
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted,
                ProjectId = model.ProjectId,
                AssignedToId = model.AssignedUserId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} created task {TaskId} in project {ProjectId}", userId, task.Id, task.ProjectId);

            return new TaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedToId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating task for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> UpdateTask(int taskId, string userId, TaskModel model, List<string> userRoles)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || (userRoles.Contains("Employee") && task.AssignedToId != userId))
            {
                _logger.LogWarning("Unauthorized task update attempt by user {UserId} for task {TaskId}", userId, taskId);
                return false;
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.IsCompleted = model.IsCompleted;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating task {TaskId} by user {UserId}", taskId, userId);
            throw;
        }
    }

    public async Task<bool> AssignTaskToEmployee(int taskId, string employeeId)
    {
        try
        {
            var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == taskId);
            var employee = await _context.Users.FindAsync(employeeId);

            if (task == null || employee == null)
            {
                _logger.LogWarning("Task or employee not found. TaskId: {TaskId}, EmployeeId: {EmployeeId}", taskId, employeeId);
                return false;
            }

            bool isEmployeeInProject = await _context.ProjectEmployees
                .AnyAsync(pe => pe.ProjectId == task.ProjectId && pe.EmployeeId == employeeId);

            if (!isEmployeeInProject)
            {
                _logger.LogWarning("Employee {EmployeeId} is not part of project {ProjectId}", employeeId, task.ProjectId);
                return false;
            }

            task.AssignedToId = employeeId;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while assigning task {TaskId} to employee {EmployeeId}", taskId, employeeId);
            throw;
        }
    }

    public async Task<bool> UnassignTaskFromEmployee(int taskId)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for unassignment.", taskId);
                return false;
            }

            task.AssignedToId = null;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unassigning task {TaskId}", taskId);
            throw;
        }
    }

    public async Task<bool> CompleteTask(int taskId, string userId, List<string> userRoles)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || (userRoles.Contains("Employee") && task.AssignedToId != userId))
            {
                _logger.LogWarning("Unauthorized task completion attempt by user {UserId} for task {TaskId}", userId, taskId);
                return false;
            }

            task.IsCompleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking task {TaskId} as complete by user {UserId}", taskId, userId);
            throw;
        }
    }

    public async Task<bool> DeleteTask(int taskId)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for deletion.", taskId);
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
            throw;
        }
    }
}
