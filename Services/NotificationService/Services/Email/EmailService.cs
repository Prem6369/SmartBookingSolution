using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NotificationService.Services.Email.Interfaces;
using NotificationService.Settings;
using Microsoft.Extensions.Options;

namespace NotificationService.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailOptions)
    {
        _emailSettings = emailOptions.Value;
    }

    public async Task SendEmailAsync(string to,string subject,string body)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_emailSettings.SenderName,_emailSettings.SenderEmail));

        email.To.Add(MailboxAddress.Parse(to));

        email.Subject = subject;

        email.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync( _emailSettings.SmtpServer, _emailSettings.Port,SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(_emailSettings.Username,_emailSettings.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);

        Console.WriteLine($"Email sent successfully to {to}");
    }
}