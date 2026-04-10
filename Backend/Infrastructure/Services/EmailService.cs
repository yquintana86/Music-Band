using Application.Abstractions.Email;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> emailOptions, ILogger<EmailService> logger)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(to) ||
            string.IsNullOrWhiteSpace(subject) ||
            string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("Email message fields are missing");
        }

        if (string.IsNullOrWhiteSpace(_emailOptions.SmtpServer) ||
            string.IsNullOrWhiteSpace(_emailOptions.From) ||
            string.IsNullOrWhiteSpace(_emailOptions.Username) ||
            string.IsNullOrWhiteSpace(_emailOptions.Password))
        {
            throw new InvalidOperationException("Email configuration is missing");
        }

        using var smtp = new SmtpClient();
        try
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_emailOptions.From!));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("html")
            {
                Text = body
            };

            await smtp.ConnectAsync(
            _emailOptions.SmtpServer,
            _emailOptions.Port,
            SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);
            await smtp.SendAsync(email);
            _logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw;
        }
        finally
        {
            if (smtp.IsConnected)
                await smtp.DisconnectAsync(true);
        }
    }
}
