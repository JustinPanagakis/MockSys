using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSys.Reporting.Data.DataClients;
using MockSys.Reporting.Data.DbContexts;

namespace MockSys.Reporting.Data.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static TBuilder AddReportingDataContext<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddNpgsqlDataSource("mocksys");
        builder.AddNpgsqlDbContext<ReportingDbContext>(connectionName: "mocksys");

        builder.Services.AddHttpClient<IFakeDataApiClient, FakeDataApiClient>(client =>
        {
            client.BaseAddress = new("https+http://mocksys-fakedataapi");
        });

        return builder;
    }
}