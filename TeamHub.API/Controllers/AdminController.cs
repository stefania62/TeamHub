using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;

namespace TeamHub.API.Controllers;

/// <summary>
///  Controller for managing admin operations.
/// </summary>
[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Administrator")] 
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _adminService.GetAllUsers();
        return Ok(users);
    }

    /// <summary>
    /// Get user by ID.
    /// </summary>
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _adminService.GetUserById(userId);
        if (user == null) return NotFound("User not found.");
        return Ok(user);
    }

    /// <summary>
    /// Create a new employee user.
    /// </summary>
    [HttpPost("create-employee")]
    public async Task<IActionResult> CreateEmployee([FromBody] UserModel model)
    {
        var user = await _adminService.CreateEmployee(model);
        if (user == null) return BadRequest("User creation failed.");
        return Ok(new { message = "Employee created successfully", user });
    }

    /// <summary>
    /// Update user details.
    /// </summary>
    [HttpPut("update-user/{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserModel model)
    {
        var updatedUser = await _adminService.UpdateUser(userId, model);
        if (updatedUser == null) return NotFound("User not found.");
        return Ok(updatedUser);
    }

    /// <summary>
    /// Delete a user.
    /// </summary>
    [HttpDelete("delete-user/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var success = await _adminService.DeleteUser(userId);
        if (!success) return NotFound("User not found.");
        return Ok(new { message = "User deleted successfully" });
    }
}
