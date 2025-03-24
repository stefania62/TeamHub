using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Application.Result;
using TeamHub.Infrastructure.Data.Context;
using TaskItem = TeamHub.Domain.Entities.TaskItem;

namespace TeamHub.Application.Services;

/// <summary>
/// Provides task operations.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ApplicationDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    ///<inheritdoc cref="ITaskService.GetUserTasks"/>
    public async Task<Result<List<TaskModel>>> GetUserTasks(string userId, List<string> userRoles)
    {
        try
        {
            List<TaskItem> tasks;

            if (userRoles.Contains(nameof(UserRole.Administrator)))
            {
                tasks = await _context.Tasks.Include(t => t.Project)
                    .Include(t => t.AssignedTo)
                    .ToListAsync();
            }
            else
            {
                tasks = await _context.Tasks
                    .Where(t => _context.ProjectEmployees
                        .Any(pe => pe.ProjectId == t.ProjectId && pe.EmployeeId == userId))
                    .Include(t => t.Project)
                    .Include(t => t.AssignedTo)
                    .ToListAsync();
            }

            var result = tasks.Select(task => new TaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                ProjectTitle = task.Project.Title,
                AssignedUserId = task.AssignedToId,
                AssignedUserName = task.AssignedTo?.FullName,
                AssignedToCurrentUser = task.AssignedToId == userId
            }).ToList();

            return Result<List<TaskModel>>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting tasks for user {UserId}", userId);
            return Result<List<TaskModel>>.Fail("Unexpected error occurred while fetching tasks.");
        }
    }

    ///<inheritdoc cref="ITaskService.CreateTask"/>
    public async Task<Result<TaskModel>> CreateTask(string userId, TaskModel model, List<string> userRoles)
    {
        try
        {
            var project = await _context.Projects
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == model.ProjectId);

            if (project == null || (userRoles.Contains(nameof(UserRole.Employee)) && !project.Employees.Any(e => e.EmployeeId == userId)))
            {
                _logger.LogWarning("Unauthorized task creation attempt by user {UserId} for project {ProjectId}", userId, model.ProjectId);
                return Result<TaskModel>.Fail("Not authorized or project not found.");
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted,
                ProjectId = model.ProjectId,
                AssignedToId = userRoles.Contains(nameof(UserRole.Employee)) ? userId : null
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} created task {TaskId} in project {ProjectId}", userId, task.Id, task.ProjectId);

            return Result<TaskModel>.Ok(new TaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedToId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating task for user {UserId}", userId);
            return Result<TaskModel>.Fail("Unexpected error occurred while creating task.");
        }
    }

    ///<inheritdoc cref="ITaskService.UpdateTask"/>
    public async Task<Result<bool>> UpdateTask(int taskId, string userId, TaskModel model, List<string> userRoles)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || (userRoles.Contains(nameof(UserRole.Employee)) && task.AssignedToId != userId))
            {
                _logger.LogWarning("Unauthorized task update attempt by user {UserId} for task {TaskId}", userId, taskId);
                return Result<bool>.Fail("Not authorized or task not found.");
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.ProjectId = model.ProjectId;
            task.IsCompleted = model.IsCompleted;
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating task {TaskId} by user {UserId}", taskId, userId);
            return Result<bool>.Fail("Unexpected error occurred while updating task.");
        }
    }

    ///<inheritdoc cref="ITaskService.CompleteTask"/>
    public async Task<Result<bool>> CompleteTask(int taskId, string userId, List<string> userRoles)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || (userRoles.Contains(nameof(UserRole.Employee)) && task.AssignedToId != userId))
            {
                _logger.LogWarning("Unauthorized task completion attempt by user {UserId} for task {TaskId}", userId, taskId);
                return Result<bool>.Fail("Not authorized or task not found.");
            }

            task.IsCompleted = true;
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking task {TaskId} as complete by user {UserId}", taskId, userId);
            return Result<bool>.Fail("Unexpected error occurred while completing task.");
        }
    }

    ///<inheritdoc cref="ITaskService.AssignEmployeeToTask"/>
    public async Task<Result<bool>> AssignEmployeeToTask(int taskId, string employeeId)
    {
        try
        {
            var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == taskId);
            var employee = await _context.Users.FindAsync(employeeId);

            if (task == null || employee == null)
            {
                _logger.LogWarning("Task or employee not found. TaskId: {TaskId}, EmployeeId: {EmployeeId}", taskId, employeeId);
                return Result<bool>.Fail("Task or employee not found.");
            }

            bool isEmployeeInProject = await _context.ProjectEmployees
                .AnyAsync(pe => pe.ProjectId == task.ProjectId && pe.EmployeeId == employeeId);

            if (!isEmployeeInProject)
            {
                _logger.LogWarning("Employee {EmployeeId} is not part of project {ProjectId}", employeeId, task.ProjectId);
                return Result<bool>.Fail("Employee is not part of the project.");
            }

            task.AssignedToId = employeeId;
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while assigning task {TaskId} to employee {EmployeeId}", taskId, employeeId);
            return Result<bool>.Fail("Unexpected error occurred while assigning task.");
        }
    }

    ///<inheritdoc cref="ITaskService.RemoveEmployeeFromTask"/>
    public async Task<Result<bool>> RemoveEmployeeFromTask(int taskId)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for unassignment.", taskId);
                return Result<bool>.Fail("Task not found.");
            }

            task.AssignedToId = null;
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unassigning task {TaskId}", taskId);
            return Result<bool>.Fail("Unexpected error occurred while unassigning task.");
        }
    }

    ///<inheritdoc cref="ITaskService.DeleteTask"/>
    public async Task<Result<bool>> DeleteTask(int taskId)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for deletion.", taskId);
                return Result<bool>.Fail("Task not found.");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
            return Result<bool>.Fail("Unexpected error occurred while deleting task.");
        }
    }
}
