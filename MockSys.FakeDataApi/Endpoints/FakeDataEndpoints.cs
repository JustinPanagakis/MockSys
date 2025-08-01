using Microsoft.AspNetCore.Mvc;
using MockSys.FakeData.Domain.Services;

namespace MockSys.FakeDataApi.Endpoints;

public static class FakeDataEndpoints
{
    public static WebApplication AddFakeDataEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/users", (IFakeDataService fakeDataService) => fakeDataService.GetUsers())
            .WithName("GetUsers")
            .WithOpenApi();

        app.MapGet("/api/v1/products", (IFakeDataService fakeDataService) => fakeDataService.GetProducts())
            .WithName("GetProducts")
            .WithOpenApi();

        app.MapGet("/api/v1/transactions", (IFakeDataService fakeDataService, [FromQuery] DateTime date) => fakeDataService.GetTransactions(date))
            .WithName("GetTransactions")
            .WithOpenApi();

        return app;
    }
}
