using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MockSys.Notification.Integration.Services.Contracts;
using MockSys.Notification.Integration.Services;
using MockSys.Notification.Domain.Services.Contracts;
using MockSys.Notification.Domain.Services;

namespace MockSys.Notification.Ctrl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddHttpClient();
                    services.AddTransient<IEmailSender, MailgunEmailSender>();
                    services.AddTransient<IEmailService, EmailService>();
                })
                .Build();

            host.Run();
        }
    }
}