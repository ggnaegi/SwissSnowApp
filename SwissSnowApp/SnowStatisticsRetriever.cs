using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwissSnowApp.Interfaces;

namespace SwissSnowApp;

public class SnowStatisticsRetriever
{
    private readonly IHttpClientManager _httpClientManager;
    private readonly ILogger<SnowStatisticsRetriever> _logger;

    public SnowStatisticsRetriever(IHttpClientManager httpClientManager, ILogger<SnowStatisticsRetriever> logger)
    {
        _httpClientManager = httpClientManager;
        _logger = logger;
    }

    [FunctionName("SnowStatisticsRetriever")]
    public async Task Run(
        [TimerTrigger("0 0 * * *")] TimerInfo timerInfo,
        [ServiceBus("snowstatisticsqueue", Connection = "ServiceBusConnection")]
        IAsyncCollector<string> chunks)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        var snowData = await _httpClientManager.RetrieveSnowData();
        var subList = snowData.Features.Split();

        foreach (var subString in subList.Select(JsonConvert.SerializeObject))
            await chunks.AddAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(subString)));
    }
}