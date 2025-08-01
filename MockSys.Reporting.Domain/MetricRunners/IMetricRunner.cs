using MockSys.Reporting.Data.Entities;

namespace MockSys.Reporting.Domain.MetricRunners;

public interface IMetricRunner
{
    Task<MetricResult> RunAsync(DateTime date);
}
