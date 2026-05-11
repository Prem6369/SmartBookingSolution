namespace NotificationService.Settings;

public class EmailSettings
{
    public string SenderEmail { get; set; } = default!;

    public string SenderName { get; set; } = default!;

    public string SmtpServer { get; set; } = default!;

    public int Port { get; set; }

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;
}