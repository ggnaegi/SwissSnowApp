using System.Collections.Generic;
using System.Threading.Tasks;
using SwissSnowApp.Dtos.Plz;

namespace SwissSnowApp.Interfaces
{
    public interface IZipCodeRetriever
    {
        Task<IEnumerable<CityDto>> RetrieveZipCode(string zipCode);
    }
}
