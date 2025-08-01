using Microsoft.Extensions.DependencyInjection;
using MockSys.Reporting.Domain.MetricRunners;

namespace MockSys.Reporting.Domain.Factories;

public interface IMetricRunnerFactory
{
    IMetricRunner GetRunner(string metricName);
}

public class MetricRunnerFactory(IServiceProvider rootServiceProvider) : IMetricRunnerFactory
{
    public IMetricRunner GetRunner(string metricName)
    {
        var scope = rootServiceProvider.CreateScope();

        var runner = scope.ServiceProvider.GetKeyedService<IMetricRunner>(metricName) ?? throw new KeyNotFoundException($"Runner not found for key '{metricName}'.");

        return new ScopedRunnerWrapper(runner, scope);
    }
}