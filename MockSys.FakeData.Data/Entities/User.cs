using System.Text.Json.Serialization;

namespace MockSys.FakeData.Data.Entities;

public record User
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
