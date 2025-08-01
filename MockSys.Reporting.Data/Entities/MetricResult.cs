using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MockSys.Reporting.Data.Entities;

public record MetricResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string MetricName { get; set; } = null!;
    public DateTime Date { get; set; }
    [Column(TypeName = "jsonb")]
    public JsonDocument Result { get; set; } = null!;
}