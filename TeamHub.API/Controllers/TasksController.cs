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
    [Authorize]
    public async Task<IActionResult> GetUserTasks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var tasks = await _taskService.GetUserTasks(userId, userRoles);
        return Ok(tasks);
    }

    [HttpPost]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> CreateTask([FromBody] TaskModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var task = await _taskService.CreateTask(userId, model, userRoles);
        if (task == null) return BadRequest("Task creation failed.");
        return CreatedAtAction(nameof(GetUserTasks), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var success = await _taskService.UpdateTask(id, userId, model, userRoles);
        if (!success) return BadRequest("Task update failed.");
        return Ok("Task updated successfully.");
    }

    [HttpPut("{id}/complete")]
    [Authorize(Roles = "Employee,Administrator")]
    public async Task<IActionResult> CompleteTask(int id, [FromBody] TaskModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var success = await _taskService.CompleteTask(id, userId, userRoles);
        if (!success) return BadRequest("Task update failed.");
        return Ok("Task updated successfully.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var success = await _taskService.DeleteTask(id);
        if (!success) return NotFound("Task not found.");
        return NoContent();
    }
}