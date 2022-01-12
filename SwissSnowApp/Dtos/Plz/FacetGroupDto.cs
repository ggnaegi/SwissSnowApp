using System.Collections.Generic;

namespace SwissSnowApp.Dtos.Plz
{
    public class FacetGroupDto
    {
        public string Name { get; set; }
        public IList<FacetDto> Facets { get; set; }
    }
}
