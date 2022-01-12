using System;
using Newtonsoft.Json;

namespace SwissSnowApp.Entities
{
    public class SnowStatisticsEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string StationId { get; set; }
        public string StationName { get; set; }
        public int AltitudeInM { get; set; }
        public long SnowInCm { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public DateTime? SnowMeasureDate { get; set; }
    }
}
