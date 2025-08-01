using MockSys.ServiceDefaults;
using MockSys.Web.Components;
using MockSys.Reporting.Data.Extensions;
using MockSys.Reporting.Domain.Extensions;
using MockSys.Web.ApiClients;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<FakeDataApiClient>(client =>
{
    client.BaseAddress = new("https+http://mocksys-fakedataapi");
});

builder.AddReportingDataContext();
builder.AddReportingServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseDatabaseAutoMigrate();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
