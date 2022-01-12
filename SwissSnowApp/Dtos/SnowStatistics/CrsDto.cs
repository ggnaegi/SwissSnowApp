using Newtonsoft.Json;

namespace SwissSnowApp.Dtos.SnowStatistics;

public class CrsDto
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("properties")] public CrsPropertiesDto Properties { get; set; }
}