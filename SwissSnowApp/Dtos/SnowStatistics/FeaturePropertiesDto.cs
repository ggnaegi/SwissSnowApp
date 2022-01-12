using System;
using Newtonsoft.Json;

namespace SwissSnowApp.Dtos.SnowStatistics
{
    public class FeaturePropertiesDto
    {
        [JsonProperty("station_name")]
        public string StationName { get; set; }

        [JsonProperty("station_symbol")]
        public long StationSymbol { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("reference_ts")]
        public string ReferenceTs { get; set; }

        [JsonProperty("altitude")]
        public string Altitude { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
