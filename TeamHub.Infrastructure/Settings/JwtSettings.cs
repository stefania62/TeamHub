namespace TeamHub.Infrastructure.Data.Settings;

/// <summary>
/// Jwt settings.
/// </summary>
public class JwtSettings 
{
    /// <summary>
    /// Gets or sets the secret key used to sign the JWT.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the issuer of the JWT (who created and signed the token).
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the audience for the JWT (who the token is intended for).
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token expiration time in minutes.
    /// </summary>
    public int ExpiryMinutes { get; set; }
}
