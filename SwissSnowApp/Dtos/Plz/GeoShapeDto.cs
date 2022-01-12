using System.Collections.Generic;

namespace SwissSnowApp.Dtos.Plz
{
    public class GeoShapeDto
    {
        public IList<IList<IList<double>>> Coordinates { get; set; }
        public string Type { get; set; }
    }
}
