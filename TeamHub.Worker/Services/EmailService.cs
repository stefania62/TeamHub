using System.Net;
using System.Net.Mail;

namespace TeamHub.Worker.Services;

/// <summary>
/// Handles sending emails using SMTP. 
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration used to retrieve SMTP settings.</param>
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc cref="EmailService.SendWelcomeEmail"/>
    public async Task SendWelcomeEmail(string toEmail, string fullName)
    {
        var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        var fromEmail = _configuration["Email:FromEmail"];
        var password = _configuration["Email:Password"];

        var subject = "Welcome to TeamHub!";
        var body = $@"
                <h2>Hello {fullName},</h2>
                <p>Welcome to <strong>TeamHub</strong>! 👋</p>
                <p>We’re thrilled to have you on the team.</p>
                <p>Start collaborating, managing tasks, and building amazing things with us.</p>
                <br/>
                <p>— The TeamHub Team</p>
            ";

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(fromEmail, password)
        };

        var mail = new MailMessage(fromEmail, toEmail, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mail);
    }
}