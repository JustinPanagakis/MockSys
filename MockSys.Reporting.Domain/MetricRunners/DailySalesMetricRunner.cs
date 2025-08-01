using Microsoft.EntityFrameworkCore;
using MockSys.Reporting.Data.DbContexts;
using MockSys.Reporting.Data.Entities;
using MockSys.Reporting.Domain.Attributes;
using System.Text.Json;

namespace MockSys.Reporting.Domain.MetricRunners;

[Metric("DailyTotalSales")]
public  class DailySalesMetricRunner(ReportingDbContext dbContext) : IMetricRunner
{
    public async Task<MetricResult> RunAsync(DateTime date)
    {
        var cleanDate = date.ToUniversalTime().Date;

        var salesTotal = await dbContext.SyncTransactions.AsNoTracking().Where(r => r.Timestamp.Date == cleanDate).SumAsync(r => r.Price + r.Tax);

        return new MetricResult()
        {
            MetricName = "DailyTotalSales",
            Date = cleanDate,
            Result = JsonSerializer.SerializeToDocument(salesTotal)
        };
    }
}
