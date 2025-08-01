using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockSys.Reporting.Data.DbContexts;

namespace MockSys.Reporting.Data.Extensions;

public static class WebAppExtensions
{
    public static WebApplication UseDatabaseAutoMigrate(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ReportingDbContext>();
            db.Database.Migrate();
        }

        return app;
    }
}
