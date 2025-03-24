namespace TeamHub.Infrastructure.Data.Settings;

/// <summary>
/// Auth settings.
/// </summary>
public class AuthSettings 
{
    /// <summary>
    /// Gets or sets the email address of the default admin user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the default admin user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username for the default admin user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the default admin user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
