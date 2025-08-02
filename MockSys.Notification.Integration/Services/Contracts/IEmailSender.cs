namespace MockSys.Notification.Integration.Services.Contracts;
public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}