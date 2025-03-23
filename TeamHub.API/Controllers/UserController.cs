using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;

namespace TeamHub.API.Controllers
{
    /// <summary>
    /// Controller managing user.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.GetProfile(userId);
            if (!result.Success) return BadRequest(new {message = result.ErrorMessage});

            return Ok(result.Data);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UserModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.UpdateProfile(userId, model);
            if (!result.Success) return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

    }
}
