using System.Collections.Generic;

namespace SwissSnowApp.Dtos.SnowStatistics;

public class GetSnowDataDto
{
    public IEnumerable<string> CitiesNames { get; set; }
}