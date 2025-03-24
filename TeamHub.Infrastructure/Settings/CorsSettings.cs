namespace TeamHub.Infrastructure.Data.Settings;

/// <summary>
/// Cors settings.
/// </summary>
public class CorsSettings
{
    /// <summary>
    /// Gets or sets the list of allowed origins for cross-origin requests.
    /// </summary>
    public string[] AllowedOrigins { get; set; } = [];
}
