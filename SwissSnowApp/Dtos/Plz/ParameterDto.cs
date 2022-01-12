using System.Collections.Generic;

namespace SwissSnowApp.Dtos.Plz
{
    public class ParameterDto
    {
        public string Dataset { get; set; }
        public int Rows { get; set; }
        public int Start { get; set; }
        public IList<string> Facet { get; set; }
        public string Format { get; set; }
        public string Timezone { get; set; }
    }
}
