using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwissSnowApp.Dtos.SnowStatistics;
using SwissSnowApp.Entities;

namespace SwissSnowApp
{
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
            [ServiceBusTrigger("snowstatisticsqueue", Connection = "ServiceBusConnection")] string base64String,
            [CosmosDB(
                databaseName: "snowstatisticsdb",
                collectionName: "snowstatisticscontainer",
                ConnectionStringSetting = "CosmosDbConnection")]
            IAsyncCollector<SnowStatisticsEntity> snowStatisticsCollection
            )
        {
            _logger.LogInformation($"Function getting new snow statistics from service bus queue");

            var bytes = Convert.FromBase64String(base64String);
            var json = Encoding.UTF8.GetString(bytes);
            var snowDataChunk = JsonConvert.DeserializeObject<IEnumerable<FeatureDto>>(json);
            var snowStatisticsEntities = _mapper.Map<IEnumerable<SnowStatisticsEntity>>(snowDataChunk);

            foreach (var entity in snowStatisticsEntities)
            {
                await snowStatisticsCollection.AddAsync(entity);
            }
        }
    }
}
