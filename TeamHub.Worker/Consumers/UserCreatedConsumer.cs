using MassTransit;
using TeamHub.Contract;
using TeamHub.Worker.Services;

namespace TeamHub.Worker.Consumers;

/// <summary>
/// Consumer that handles <see cref="UserCreatedEvent"/> messages from the message queue.
/// </summary>
public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreatedConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    public UserCreatedConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Consumes the <see cref="UserCreatedEvent"/> and sends a welcome email to the new user.
    /// </summary>
    /// <param name="context">The message context containing event data.</param>
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var user = context.Message;
        await _emailService.SendWelcomeEmail(user.Email, user.FullName);
    }
}