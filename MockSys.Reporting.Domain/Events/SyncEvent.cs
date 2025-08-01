using System.Text.Json.Serialization;

namespace MockSys.Reporting.Domain.Events;

public record SyncEvent
{
    [JsonPropertyName("syncDate")]
    public required DateTime SyncDate { get; set; }
}