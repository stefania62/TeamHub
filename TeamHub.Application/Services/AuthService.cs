using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Models;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace TeamHub.Application.Services;

/// <summary>
/// Provides authentication operations.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtOptions,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
    }

    ///<inheritdoc cref="IAuthService.AuthenticateUser"/>
    public async Task<string> AuthenticateUser(LoginModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            _logger.LogWarning("Invalid login request. Email or password is missing.");
            return null;
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("User not found with email: {Email}", model.Email);
            return null;
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            _logger.LogWarning("Invalid password attempt for user: {Email}", model.Email);
            return null;
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        _logger.LogInformation("User authenticated. Generating JWT token for user: {Email}", user.Email);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        try
        {
            var token = GenerateJwtToken(authClaims);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JWT token for user: {Email}", user.Email);
            throw;
        }
    }

    /// <summary>
    /// Generates a signed JWT token using the provided claims.
    /// </summary>
    /// <param name="authClaims">Claims to include in the token.</param>
    /// <returns>Signed JWT token as a string.</returns>
    /// <exception cref="ArgumentException">Thrown if JWT secret is missing or invalid.</exception>
    private string GenerateJwtToken(IEnumerable<Claim> authClaims)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Secret) || _jwtSettings.Secret.Length < 32)
        {
            _logger.LogError("JWT secret key is missing or too short.");
            throw new ArgumentException("JWT secret key must be at least 32 characters long.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
