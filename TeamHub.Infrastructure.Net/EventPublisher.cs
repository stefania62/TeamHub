using MassTransit;
using TeamHub.Application.Interfaces;
using TeamHub.Contract;

namespace TeamHub.Infrastructure.Net;

/// <summary>
/// Publish events to the message broker.
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventPublisher"/> class.
    /// </summary>
    /// <param name="publishEndpoint">
    /// The MassTransit publish endpoint.
    /// </param>
    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    /// <inheritdoc cref="IEventPublisher.PublishUserCreatedAsync"/>
    public async Task PublishUserCreatedAsync(string userId, string email, string fullName)
    {
        await _publishEndpoint.Publish(new UserCreatedEvent
        {
            UserId = userId,
            Email = email,
            FullName = fullName
        });
    }
}
