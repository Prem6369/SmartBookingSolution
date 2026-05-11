using NotificationService.Services.Email.Interfaces;

namespace NotificationService.Services.Email;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
    {
        Console.WriteLine("========= EMAIL SENT =========");

        Console.WriteLine($"To: {to}");

        Console.WriteLine($"Subject: {subject}");

        Console.WriteLine($"Body: {body}");

        Console.WriteLine("==============================");

        await Task.CompletedTask;
    }
}