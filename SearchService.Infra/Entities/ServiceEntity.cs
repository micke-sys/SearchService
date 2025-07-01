using System.Text.Json.Serialization;

namespace SearchService.Infra.Entities;

public class ServiceEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("position")]
    public PositionEntity Position { get; set; } = default!;
}

public class PositionEntity
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
}