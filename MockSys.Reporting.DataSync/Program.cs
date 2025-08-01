using MockSys.ServiceDefaults;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSys.Reporting.Data.Extensions;
using MockSys.Reporting.Domain.Extensions;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddReportingDataContext();
builder.AddReportingServices();

builder.Services.AddSingleton(sp =>
{
    var cs = Environment.GetEnvironmentVariable("mocksys-servicebus");
    return new ServiceBusClient(cs);
});

builder.ConfigureFunctionsWebApplication();

await builder.Build().RunAsync();
