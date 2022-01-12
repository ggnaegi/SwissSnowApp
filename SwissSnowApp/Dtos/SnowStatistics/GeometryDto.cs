using Newtonsoft.Json;

namespace SwissSnowApp.Dtos.SnowStatistics
{
    public class GeometryDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; }
    }
}
