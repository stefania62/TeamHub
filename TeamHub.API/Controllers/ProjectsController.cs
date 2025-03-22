using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;

namespace TeamHub.API.Controllers;

/// <summary>
/// Controller managing project operations.
/// </summary>
[Route("api/projects")]
[ApiController]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRoles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(r => r.Value).ToList();

        var result = await _projectService.GetProjects(userId, userRoles);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectModel model)
    {
        var result = await _projectService.CreateProject(model);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Project created successfully", project = result.Data });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectModel model)
    {
        var result = await _projectService.UpdateProject(id, model);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Project updated successfully", project = result.Data });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProject(id);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Project deleted successfully" });
    }

    [HttpPost("{projectId}/assign/{employeeId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AssignEmployeeToProject(int projectId, string employeeId)
    {
        var result = await _projectService.AssignEmployeeToProject(projectId, employeeId);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Employee assigned successfully." });
    }

    [HttpDelete("{projectId}/remove/{employeeId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RemoveEmployeeFromProject(int projectId, string employeeId)
    {
        var result = await _projectService.RemoveEmployeeFromProject(projectId, employeeId);
        if (!result.Success) return NotFound(new { message = result.ErrorMessage });

        return Ok(new { message = "Employee removed successfully." });
    }
}
