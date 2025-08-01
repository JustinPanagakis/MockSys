namespace MockSys.Reporting.Domain.MetricRunners;

public class MetricRunnerManifest
{
    public List<string> Metrics { get; private set; } = [];

    public void AddMetric(string metricName)
    {
        Metrics.Add(metricName);
    }
}