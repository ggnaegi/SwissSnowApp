using System.Collections.Generic;

namespace SwissSnowApp.Dtos.Plz
{
    public class PostZipCodeDto
    {
        public int Nhits { get; set; }
        public ParameterDto Parameters { get; set; }
        public IList<RecordDto> Records { get; set; }
        public IList<FacetGroupDto> FacetGroups { get; set; }
    }
}
