using TeamHub.Application.Models;
using TeamHub.Application.Result;

namespace TeamHub.Application.Interfaces;

public interface ITaskService
{
    Task<Result<List<TaskModel>>> GetUserTasks(string userId, List<string> userRoles);
    Task<Result<TaskModel>> CreateTask(string userId, TaskModel model, List<string> userRoles);
    Task<Result<bool>> UpdateTask(int taskId, string userId, TaskModel model, List<string> userRoles);
    Task<Result<bool>> AssignEmployeeToTask(int taskId, string employeeId);
    Task<Result<bool>> RemoveEmployeeFromTask(int taskId);
    Task<Result<bool>> CompleteTask(int taskId, string userId, List<string> userRoles);
    Task<Result<bool>> DeleteTask(int taskId);
}