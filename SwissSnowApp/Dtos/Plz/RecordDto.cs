using System;

namespace SwissSnowApp.Dtos.Plz;

public class RecordDto
{
    public string Datasetid { get; set; }
    public string Recordid { get; set; }
    public FieldDto Fields { get; set; }
    public GeometryDto Geometry { get; set; }
    public DateTime RecordTimestamp { get; set; }
}