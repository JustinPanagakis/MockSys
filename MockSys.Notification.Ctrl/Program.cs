using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSys.Notification.Domain.Services;
using MockSys.Notification.Domain.Services.Contracts;
using MockSys.Notification.Integration.Services.Contracts;
using MockSys.Notification.Integration.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddHttpClient();
builder.Services.AddTransient<IEmailSender, MailgunEmailSender>();
builder.Services.AddTransient<IEmailService, EmailService>();


builder.Build().Run();
