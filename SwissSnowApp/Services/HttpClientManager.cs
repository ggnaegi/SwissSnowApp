using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwissSnowApp.Dtos.Plz;
using SwissSnowApp.Dtos.SnowStatistics;
using SwissSnowApp.Interfaces;

namespace SwissSnowApp.Services
{
    /// <summary>
    /// HttpClientManager, Retrieving data about cities using the zip code as
    /// reference, calling an API from the swiss post.
    /// </summary>
    public class HttpClientManager : IHttpClientManager
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HttpClientManager> _logger;

        public HttpClientManager(IMapper mapper, ILogger<HttpClientManager> logger)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieving cities referenced by zip code
        /// calling swiss post api
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CityDto>> RetrieveCities(string zipCode)
        {
            _logger.LogInformation($"Now retrieving cities with zip code {zipCode}");
            var httpClient = new HttpClient();

            var resourceUri = Environment.GetEnvironmentVariable("PostPlzEndpoint");
            var result = await httpClient.GetAsync($"{resourceUri}{zipCode}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var bodyString = await result.Content.ReadAsStringAsync();
            var postZipCode = JsonConvert.DeserializeObject<PostZipCodeDto>(bodyString);

            return _mapper.Map<IEnumerable<CityDto>>(postZipCode.Records);
        }
        
        /// <summary>
        /// Retrieving snow data from meteoswiss
        /// </summary>
        /// <returns></returns>
        public async Task<SnowDataDto> RetrieveSnowData()
        {
            _logger.LogInformation("Now retrieving snow data from meteo swiss");
            var httpClient = new HttpClient();

            var resourceUri = Environment.GetEnvironmentVariable("MeteoSwissEndpoint");
            var result = await httpClient.GetAsync(resourceUri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            try
            {
                var bodyString = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SnowDataDto>(bodyString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
