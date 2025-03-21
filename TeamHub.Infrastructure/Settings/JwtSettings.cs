﻿namespace TeamHub.Infrastructure.Settings;

/// <summary>
/// Jwt settings.
/// </summary>
public class JwtSettings 
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
    public string AllowedOrigins { get; set; }
}
