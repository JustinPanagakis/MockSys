using MockSys.ServiceDefaults;
using MockSys.FakeDataApi.Endpoints;
using MockSys.FakeData.Data.Providers;
using MockSys.FakeData.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<JsonDataProvider>();
builder.Services.AddTransient<IFakeDataService, FakeDataService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddFakeDataEndpoints();

app.Run();
