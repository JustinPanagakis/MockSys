using MockSys.Notification.Domain.Services.Contracts;
using MockSys.Notification.Integration.Services.Contracts;

namespace MockSys.Notification.Domain.Services;
public class EmailService(IEmailSender _emailSender) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // other business logic to go here.. logging, validation, etc.
        await _emailSender.SendAsync(to, subject, body);
    }
}