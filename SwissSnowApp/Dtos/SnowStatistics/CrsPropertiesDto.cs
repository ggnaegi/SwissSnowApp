using Newtonsoft.Json;

namespace SwissSnowApp.Dtos.SnowStatistics;

public class CrsPropertiesDto
{
    [JsonProperty("name")] public string Name { get; set; }
}