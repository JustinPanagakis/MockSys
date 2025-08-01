using System.Text.Json.Serialization;

namespace MockSys.Reporting.Domain.Events;

public record MetricEvent
{
    [JsonPropertyName("metricName")]
    public required string MetricName { get; set; }
    [JsonPropertyName("date")]
    public required DateTime Date { get; set; }
}