using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;

namespace TeamHub.API.Controllers;

/// <summary>
/// Controller managing task operations.
/// </summary>
[Route("api/tasks")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> GetTasks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var result = await _taskService.GetUserTasks(userId, userRoles);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> CreateTask([FromBody] TaskModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var result = await _taskService.CreateTask(userId, model, userRoles);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });
        return Ok(new { message = "Task created successfully", task = result.Data });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var result = await _taskService.UpdateTask(id, userId, model, userRoles);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });
        return Ok(new { message = "Task updated successfully" });
    }

    [HttpPut("mark-complete/{id}")]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var result = await _taskService.CompleteTask(id, userId, userRoles);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });
        return Ok(new { message = "Task marked as completed" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _taskService.DeleteTask(id);
        if (!result.Success) return NotFound(new { message = result.ErrorMessage });
        return Ok(new { message = "Task deleted successfully" });
    }

    [HttpPost("{taskId}/assign/{employeeId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AssignEmployeeToTask(int taskId, string employeeId)
    {
        var result = await _taskService.AssignEmployeeToTask(taskId, employeeId);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Employee assigned successfully." });
    }

    [HttpDelete("{taskId}/remove/{employeeId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RemoveEmployeeFromTask(int taskId, string employeeId)
    {
        var result = await _taskService.RemoveEmployeeFromTask(taskId);
        if (!result.Success) return NotFound(new { message = result.ErrorMessage });

        return Ok(new { message = "Employee removed successfully." });
    }
}