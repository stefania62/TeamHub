namespace TeamHub.Worker.Services;

/// <summary>
/// Defines an interface for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends a personalized welcome email to a newly registered user.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="fullName">The full name of the recipient, used to personalize the message.</param>
    /// <returns>A task representing the asynchronous send operation.</returns>
    Task SendWelcomeEmail(string toEmail, string fullName);
}