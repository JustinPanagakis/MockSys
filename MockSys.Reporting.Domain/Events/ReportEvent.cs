using System.Text.Json.Serialization;

namespace MockSys.Reporting.Domain.Events;

public class ReportEvent
{
    [JsonPropertyName("reportDate")]
    public required DateTime ReportDate { get; set; }
}