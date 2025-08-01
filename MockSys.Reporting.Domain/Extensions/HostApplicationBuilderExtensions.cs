using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSys.Reporting.Domain.Attributes;
using MockSys.Reporting.Domain.Factories;
using MockSys.Reporting.Domain.MetricRunners;
using MockSys.Reporting.Domain.Services;
using System.Reflection;

namespace MockSys.Reporting.Domain.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static TBuilder AddReportingServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddTransient<ITransactionSyncService, TransactionSyncService>();

        builder.Services.AddSingleton<IMetricRunnerFactory, MetricRunnerFactory>();

        builder.Services.AddTransient<IMetricStorageService, MetricStorageService>();

        builder.RegisterMetricRunners();

        return builder;
    }

    private static TBuilder RegisterMetricRunners<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var runnerInterface = typeof(IMetricRunner);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !(a.FullName ?? string.Empty).StartsWith("System") && !(a.FullName ?? string.Empty).StartsWith("Microsoft"))
            .ToList();

        var implementationTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && runnerInterface.IsAssignableFrom(t) && t.Name != "ScopedRunnerWrapper")
            .ToList();

        var metricManifest = new MetricRunnerManifest();

        foreach (var implementationType in implementationTypes)
        {
            var key = implementationType.GetCustomAttribute<MetricAttribute>()?.Name ?? implementationType.Name;

            metricManifest.AddMetric(key);
            builder.Services.AddKeyedTransient(serviceType: typeof(IMetricRunner), implementationType: implementationType, serviceKey: key);
        }

        builder.Services.AddSingleton(metricManifest);

        return builder;
    }
}
