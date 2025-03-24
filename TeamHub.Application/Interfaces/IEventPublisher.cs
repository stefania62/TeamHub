namespace TeamHub.Application.Interfaces;

/// <summary>
/// Defines an interface for publishing integration events.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes a UserCreated event to the message queue.
    /// </summary>
    /// <param name="userId">The ID of the user who was created.</param>
    /// <param name="email">The email address of the new user.</param>
    /// <param name="fullName">The full name of the new user.</param>
    /// <returns>A task representing the asynchronous publish operation.</returns>
    Task PublishUserCreatedAsync(string userId, string email, string fullName);
}

