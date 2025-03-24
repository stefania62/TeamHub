namespace TeamHub.Contract;

/// <summary>
/// Represents the event data published when a new user is created.
/// </summary>
public class UserCreatedEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly created user.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the new user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the new user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}