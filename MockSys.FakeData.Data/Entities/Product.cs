using System.Text.Json.Serialization;

namespace MockSys.FakeData.Data.Entities;

public record Product
{

    [JsonPropertyName("id")]
    public required Guid Id { get; set; }
    [JsonPropertyName("brand")]
    public required string Brand { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("price")]
    public required decimal Price { get; set; }
    [JsonPropertyName("category")]
    public required string Category { get; set; }
}
