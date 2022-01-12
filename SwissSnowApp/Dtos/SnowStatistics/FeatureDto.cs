using Newtonsoft.Json;

namespace SwissSnowApp.Dtos.SnowStatistics;

public class FeatureDto
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("geometry")] public GeometryDto Geometry { get; set; }

    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("properties")] public FeaturePropertiesDto Properties { get; set; }
}