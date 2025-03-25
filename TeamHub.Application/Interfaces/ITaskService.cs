using TeamHub.Application.Models;
using TeamHub.Application.Result;

namespace TeamHub.Application.Interfaces;

/// <summary>
/// Defines operations related to task management.
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Retrieves tasks based on the user's ID and roles.
    /// </summary>
    /// <param name="userId">The ID of the requesting user.</param>
    /// <param name="userRoles">The roles assigned to the user.</param>
    /// <returns>A result containing a list of tasks accessible to the user.</returns>
    Task<Result<List<TaskModel>>> GetUserTasks(string userId, List<string> userRoles);

    /// <summary>
    /// Creates a new task under a project.
    /// </summary>
    /// <param name="userId">The ID of the user creating the task.</param>
    /// <param name="model">The task data to be created.</param>
    /// <param name="userRoles">The roles of the user creating the task.</param>
    /// <returns>A result containing the created task.</returns>
    Task<Result<TaskModel>> CreateTask(string userId, TaskModel model, List<string> userRoles);

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="taskId">The ID of the task to update.</param>
    /// <param name="userId">The ID of the user attempting the update.</param>
    /// <param name="model">The updated task data.</param>
    /// <param name="userRoles">The roles of the user performing the update.</param>
    /// <returns>A result indicating whether the update was successful.</returns>
    Task<Result<bool>> UpdateTask(int taskId, string userId, TaskModel model, List<string> userRoles);

    /// <summary>
    /// Marks a task as completed.
    /// </summary>
    /// <param name="taskId">The ID of the task to complete.</param>
    /// <param name="userId">The ID of the user completing the task.</param>
    /// <param name="userRoles">The roles of the user completing the task.</param>
    /// <returns>A result indicating whether the completion was successful.</returns>
    Task<Result<bool>> CompleteTask(int taskId, string userId, List<string> userRoles);

    /// <summary>
    /// Assigns an employee to a task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="employeeId">The ID of the employee to assign.</param>
    /// <returns>A result indicating whether the assignment was successful.</returns>
    Task<Result<bool>> AssignEmployeeToTask(int taskId, string employeeId);

    /// <summary>
    /// Removes the assigned employee from a task.
    /// </summary>
    /// <param name="taskId">The ID of the task to update.</param>
    /// <param name="employeeId">The ID of the employee to be removed from task.</param>
    /// <returns>A result indicating whether the removal was successful.</returns>
    Task<Result<bool>> RemoveEmployeeFromTask(int taskId, string employeeId);

    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="taskId">The ID of the task to delete.</param>
    /// <returns>A result indicating whether the deletion was successful.</returns>
    Task<Result<bool>> DeleteTask(int taskId);
}
