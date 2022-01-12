using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwissSnowApp.Dtos.SnowStatistics;
using SwissSnowApp.Entities;

namespace SwissSnowApp;

public class SnowStatisticsParser
{
    private readonly ILogger<SnowStatisticsParser> _logger;
    private readonly IMapper _mapper;

    public SnowStatisticsParser(IMapper mapper, ILogger<SnowStatisticsParser> logger)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [FunctionName("SnowStatisticsParser")]
    public async Task ParseSnowStatistics(
        [ServiceBusTrigger("snowstatisticsqueue", Connection = "ServiceBusConnection")]
        string base64String,
        [CosmosDB(
            "snowstatisticsdb",
            "snowstatisticscontainer",
            Connection = "CosmosDbConnection",
            Id = "Id")]
        CosmosClient cosmosClient
    )
    {
        _logger.LogInformation("Function getting new snow statistics from service bus queue");

        var bytes = Convert.FromBase64String(base64String);
        var json = Encoding.UTF8.GetString(bytes);
        var snowDataChunk = JsonConvert.DeserializeObject<IEnumerable<FeatureDto>>(json);
        var snowStatisticsEntities = _mapper.Map<IEnumerable<SnowStatisticsEntity>>(snowDataChunk);

        var container = cosmosClient.GetDatabase("snowstatisticsdb").GetContainer("snowstatisticscontainer");
        foreach (var entity in snowStatisticsEntities.Where(x => x.SnowMeasureDate != null))
        {
            if (await EntryExists(entity.StationName, entity.SnowMeasureDate.Value, container)) continue;

            await container.CreateItemAsync(entity);
        }
    }

    /// <summary>
    ///     Checking if measure has already been imported
    /// </summary>
    /// <param name="stationName"></param>
    /// <param name="snowMeasureDate"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    private async Task<bool> EntryExists(string stationName, DateTime snowMeasureDate, Container container)
    {
        var query = new QueryDefinition(
                "SELECT * FROM c Where c.StationName = @name and c.SnowMeasureDate = @measureDate")
            .WithParameter("@name", stationName)
            .WithParameter("@measureDate", snowMeasureDate);

        using var resultSet = container.GetItemQueryIterator<SnowStatisticsEntity>(query);
        var nextResult = await resultSet.ReadNextAsync();
        var foundValue = nextResult.FirstOrDefault();
        return foundValue != null;
    }
}