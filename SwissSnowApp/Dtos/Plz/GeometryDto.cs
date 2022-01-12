using System.Collections.Generic;

namespace SwissSnowApp.Dtos.Plz;

public class GeometryDto
{
    public string Type { get; set; }
    public IList<double> Coordinates { get; set; }
}