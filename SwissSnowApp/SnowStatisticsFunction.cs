using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwissSnowApp.Dtos.SnowStatistics;
using SwissSnowApp.Entities;

namespace SwissSnowApp;

public class SnowStatisticsFunction
{
    private readonly ILogger<SnowStatisticsFunction> _logger;
    private readonly IMapper _mapper;

    public SnowStatisticsFunction(IMapper mapper, ILogger<SnowStatisticsFunction> log)
    {
        _logger = log;
        _mapper = mapper;
    }

    [FunctionName("SnowStatisticsFunction")]
    [OpenApiOperation("Run", new[] {"GetSnowStatistics"})]
    [OpenApiRequestBody("application/json", typeof(GetSnowDataDto))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json",
        typeof(IEnumerable<SnowStatisticsDto>), Description = "The OK response")]
    public async Task<ActionResult<IEnumerable<SnowStatisticsDto>>> GetSnowStatistics(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req,
        [CosmosDB(
            "snowstatisticsdb",
            "snowstatisticscontainer",
            Connection = "CosmosDbConnection")]
        CosmosClient cosmosClient)
    {
        var content = await new StreamReader(req.Body).ReadToEndAsync();

        var getDto = JsonConvert.DeserializeObject<GetSnowDataDto>(content);

        var container = cosmosClient
            .GetDatabase("snowstatisticsdb")
            .GetContainer("snowstatisticscontainer");

        var feedIterator = container
            .GetItemLinqQueryable<SnowStatisticsEntity>()
            .Where(p => getDto.CitiesNames.Contains(p.StationName))
            .ToFeedIterator();

        var statistics = new List<SnowStatisticsDto>();
        while (feedIterator.HasMoreResults)
            statistics.AddRange(
                from SnowStatisticsEntity result
                    in await feedIterator.ReadNextAsync()
                select _mapper.Map<SnowStatisticsDto>(result)
            );

        return statistics;
    }
}