using Microsoft.Extensions.DependencyInjection;
using MockSys.Reporting.Data.Entities;

namespace MockSys.Reporting.Domain.MetricRunners;

public class ScopedRunnerWrapper(IMetricRunner inner, IServiceScope scope) : IMetricRunner, IDisposable
{
    public async Task<MetricResult> RunAsync(DateTime date) => await inner.RunAsync(date);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        scope.Dispose();
    }
}