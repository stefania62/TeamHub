using TeamHub.API.Models;

namespace TeamHub.Application.Interfaces;

public interface ITaskService
{
    Task<List<TaskModel>> GetUserTasks(string userId, List<string> userRoles);
    Task<TaskModel> CreateTask(string userId, TaskModel model, List<string> userRoles);
    Task<bool> UpdateTask(int taskId, string userId, TaskModel model, List<string> userRoles);
    Task<bool> AssignTaskToEmployee(int taskId, string employeeId);
    Task<bool> UnassignTaskFromEmployee(int taskId);
    Task<bool> CompleteTask(int taskId, string userId, List<string> userRoles);
    Task<bool> DeleteTask(int taskId);
}