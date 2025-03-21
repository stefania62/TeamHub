using Microsoft.EntityFrameworkCore;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data;

namespace TeamHub.Application.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskModel>> GetUserTasks(string userId, List<string> userRoles)
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

    public async Task<TaskModel> CreateTask(string userId, TaskModel model, List<string> userRoles)
    {
        var project = await _context.Projects
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == model.ProjectId);

        if (project == null || (userRoles.Contains("Employee") && !project.Employees.Any(e => e.EmployeeId == userId)))
            return null;

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

    public async Task<bool> UpdateTask(int taskId, string userId, TaskModel model, List<string> userRoles)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null || (userRoles.Contains("Employee") && task.AssignedToId != userId))
            return false;

        task.Title = model.Title;
        task.Description = model.Description;
        task.IsCompleted = model.IsCompleted;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AssignTaskToEmployee(int taskId, string employeeId)
    {
        var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == taskId);
        var employee = await _context.Users.FindAsync(employeeId);

        if (task == null || employee == null)
            return false;

        bool isEmployeeInProject = await _context.ProjectEmployees
            .AnyAsync(pe => pe.ProjectId == task.ProjectId && pe.EmployeeId == employeeId);

        if (!isEmployeeInProject)
            return false;

        task.AssignedToId = employeeId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnassignTaskFromEmployee(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null)
            return false;

        task.AssignedToId = null;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteTask(int taskId, string userId, List<string> userRoles)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null || (userRoles.Contains("Employee") && task.AssignedToId != userId))
            return false;

        task.IsCompleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTask(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}
