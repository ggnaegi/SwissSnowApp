using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SwissSnowApp.Common.CacheManager;
using SwissSnowApp.Dtos.Plz;
using SwissSnowApp.Interfaces;

namespace SwissSnowApp.Services;

public class ZipCodeRetriever : IZipCodeRetriever
{
    private readonly ICacheManager _cacheManager;
    private readonly IHttpClientManager _httpClientManager;

    public ZipCodeRetriever(IHttpClientManager httpClientManager, ICacheManager cacheManager)
    {
        _httpClientManager = httpClientManager;
        _cacheManager = cacheManager;
    }

    public async Task<IEnumerable<CityDto>> RetrieveZipCode(string zipCode)
    {
        var citiesFromCache = await _cacheManager.Get<CityDto>(zipCode);
        if (citiesFromCache != null) return citiesFromCache;

        var result = (await _httpClientManager.RetrieveCities(zipCode)).ToArray();
        if (!result.Any()) return result;

        await _cacheManager.InsertOrUpdate(zipCode, result);
        return result;
    }
}