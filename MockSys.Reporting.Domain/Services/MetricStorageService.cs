using MockSys.Reporting.Data.DbContexts;
using MockSys.Reporting.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSys.Reporting.Domain.Services;

public interface IMetricStorageService
{
    Task StoreMetricResultAsync(MetricResult metricResult, DateTime date);
}

public class MetricStorageService(ReportingDbContext dbContext) : IMetricStorageService
{
    public async Task StoreMetricResultAsync(MetricResult metricResult, DateTime date)
    {
        if (metricResult.Date != date)
        {
            throw new InvalidOperationException("unexpected metric result date");
        }

        await dbContext.MetricResults.AddAsync(metricResult);
        await dbContext.SaveChangesAsync();
    }
}
