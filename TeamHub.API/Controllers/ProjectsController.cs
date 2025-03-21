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

        var projects = await _projectService.GetProjects(userId, userRoles);
        return Ok(projects);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectModel model)
    {
        if (string.IsNullOrEmpty(model.Name))
            return BadRequest("Project name is required.");

        var project = await _projectService.CreateProject(model);
        return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectModel model)
    {
        if (string.IsNullOrEmpty(model.Name))
            return BadRequest("Project name is required.");

        var updatedProject = await _projectService.UpdateProject(id, model);
        if (updatedProject == null)
            return NotFound("Project not found.");

        return Ok(updatedProject);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var success = await _projectService.DeleteProject(id);
        if (!success) return BadRequest("Cannot delete project with open tasks.");

        return NoContent();
    }

    [HttpPost("{projectId}/assign/{employeeId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AssignEmployeeToProject(int projectId, string employeeId)
    {
        var success = await _projectService.AssignEmployeeToProject(projectId, employeeId);
        if (!success) return BadRequest("Employee is already assigned or project not found.");

        return Ok("Employee assigned successfully.");
    }

    [HttpDelete("{projectId}/remove/{employeeId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RemoveEmployeeFromProject(int projectId, string employeeId)
    {
        var success = await _projectService.RemoveEmployeeFromProject(projectId, employeeId);
        if (!success) return NotFound("Employee is not part of this project.");

        return Ok("Employee removed successfully.");
    }
}

