﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;

namespace TeamHub.API.Controllers;

/// <summary>
///  Controller for managing admin operations.
/// </summary>
[Route("api/admin")]
[ApiController]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/>.
    /// </summary>
    /// <param name="adminService">An implementation of <see cref="IAdminService"/>.</param>
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _adminService.GetAllUsers(userId);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });
        return Ok(result.Data);
    }

    /// <summary>
    /// Get user by ID.
    /// </summary>
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var result = await _adminService.GetUserById(userId);
        if (!result.Success) return NotFound(new { message = result.ErrorMessage });
        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new employee user.
    /// </summary>
    [HttpPost("create-employee")]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    public async Task<IActionResult> CreateEmployee([FromForm] UserModel model)
    {
        var result = await _adminService.CreateEmployee(model);
        if (!result.Success) return BadRequest(new { message = result.ErrorMessage });
        return Ok(new { message = "Employee created successfully", user = result.Data });
    }

    /// <summary>
    /// Update user details.
    /// </summary>
    [HttpPut("update-user/{userId}")]
   [Authorize(Roles = nameof(UserRole.Administrator))]
    public async Task<IActionResult> UpdateUser(string userId, [FromForm] UserModel model)
    {
        var result = await _adminService.UpdateUser(userId, model);
        if (!result.Success) return NotFound(new { message = result.ErrorMessage });
        return Ok(result.Data);
    }

    /// <summary>
    /// Delete a user.
    /// </summary>
    [HttpDelete("delete-user/{userId}")]
   [Authorize(Roles = nameof(UserRole.Administrator))]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _adminService.DeleteUser(userId);
        if (!result.Success) return NotFound(new { message = result.ErrorMessage });
        return Ok(new { message = "User deleted successfully" });
    }
}
