using System;

namespace SwissSnowApp.Dtos.SnowStatistics;

public class SnowStatisticsDto
{
    public string StationId { get; set; }
    public string StationName { get; set; }
    public int AltitudeInM { get; set; }
    public long SnowInCm { get; set; }
    public DateTime? SnowMeasureDate { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
}