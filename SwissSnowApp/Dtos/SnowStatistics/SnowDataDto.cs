using System;
using Newtonsoft.Json;

namespace SwissSnowApp.Dtos.SnowStatistics
{
    public class SnowDataDto
    {
        [JsonProperty("crs")]
        public CrsDto Crs { get; set; }

        [JsonProperty("license")]
        public Uri? License { get; set; }

        [JsonProperty("mapname")]
        public string Mapname { get; set; }

        [JsonProperty("map_long_name")]
        public string MapLongName { get; set; }

        [JsonProperty("map_short_name")]
        public string MapShortName { get; set; }

        [JsonProperty("map_abstract")]
        public string MapAbstract { get; set; }

        [JsonProperty("creation_time")]
        public string CreationTime { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public FeatureDto[] Features { get; set; }
    }
}
