using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwissSnowApp.Dtos.SnowStatistics;
using SwissSnowApp.Entities;

namespace SwissSnowApp
{
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
        [OpenApiOperation(operationId: "Run", tags: new[] {"name"})]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetSnowDataDto) )]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(IEnumerable<SnowStatisticsDto>), Description = "The OK response")]
        public async Task<ActionResult<IEnumerable<SnowStatisticsDto>>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            [CosmosDB(
                databaseName: "snowstatisticsdb",
                collectionName: "snowstatisticscontainer",
                ConnectionStringSetting = "CosmosDbConnection")]
            DocumentClient client)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            var getDto = JsonConvert.DeserializeObject<GetSnowDataDto>(content);
            var collectionUri = UriFactory.CreateDocumentCollectionUri("snowstatisticsdb", "snowstatisticscontainer");
            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            var query = client.CreateDocumentQuery<SnowStatisticsEntity>(collectionUri, option)
                .Where(p => getDto.CitiesNames.Contains(p.StationName))
                .AsDocumentQuery();

            var statistics = new List<SnowStatisticsDto>();
            while (query.HasMoreResults)
            {
                statistics.AddRange(
                    from SnowStatisticsEntity result 
                        in await query.ExecuteNextAsync<SnowStatisticsEntity>() 
                    select _mapper.Map<SnowStatisticsDto>(result)
                    );
            }

            return statistics;
        }
    }
}

