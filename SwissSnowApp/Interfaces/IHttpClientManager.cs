using System.Collections.Generic;
using System.Threading.Tasks;
using SwissSnowApp.Dtos.Plz;
using SwissSnowApp.Dtos.SnowStatistics;

namespace SwissSnowApp.Interfaces;

public interface IHttpClientManager
{
    public Task<IEnumerable<CityDto>> RetrieveCities(string zipCode);
    public Task<SnowDataDto> RetrieveSnowData();
}