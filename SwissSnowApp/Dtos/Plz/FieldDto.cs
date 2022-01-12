using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable IdentifierTypo

namespace SwissSnowApp.Dtos.Plz
{
    public class FieldDto
    {
        public string Ortbez27 { get; set; }
        public IList<double> GeoPoint2D { get; set; }
        public string PlzCoff { get; set; }
        public string RecArt { get; set; }
        public int Sprachcode { get; set; }
        public int Bfsnr { get; set; }
        public string Kanton { get; set; }
        public string GiltAbDat { get; set; }
        public int Onrp { get; set; }
        public string Postleitzahl { get; set; }
        public int Gplz { get; set; }
        public int PlzBriefzust { get; set; }
        public string Ortbez18 { get; set; }
        public int BriefzDurch { get; set; }
        public string PlzZz { get; set; }
        public GeoShapeDto GeoShape { get; set; }

        [JsonProperty("plz_typ")]
        public int PlzTyp { get; set; }
    }
}
