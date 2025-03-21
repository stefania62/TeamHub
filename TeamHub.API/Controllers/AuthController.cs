using Microsoft.AspNetCore.Mvc;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;

namespace TeamHub.API.Controllers
{
    /// <summary>
    /// Controller managing auth operations.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="model">Login model</param>
        /// <returns>JWT Token.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.AuthenticateUser(model);
            if (token == null)
                return Unauthorized("Invalid login attempt.");

            return Ok(new { token });
        }
    }
}
