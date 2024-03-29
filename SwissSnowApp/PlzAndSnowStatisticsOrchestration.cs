using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SwissSnowApp.Dtos.Plz;
using SwissSnowApp.Dtos.PlzAndSnowStatistics;
using SwissSnowApp.Dtos.SnowStatistics;

namespace SwissSnowApp;

public class PlzAndSnowStatisticsOrchestration
{
    private readonly ILogger<PlzAndSnowStatisticsOrchestration> _logger;

    public PlzAndSnowStatisticsOrchestration(ILogger<PlzAndSnowStatisticsOrchestration> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Starting durable function (background task),
    ///     calling RunOrchestrator
    /// </summary>
    /// <param name="req"></param>
    /// <param name="durableClient"></param>
    /// <returns></returns>
    [FunctionName("PlzAndSnowStatisticsGet")]
    [OpenApiOperation("Run", new[] {"DurableFunction.GetPlzAndSnowStatistics"})]
    [OpenApiParameter("zip_code", In = ParameterLocation.Query, Required = true, Type = typeof(string),
        Description = "The swiss zip code")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(IEnumerable<SnowStatisticsDto>),
        Description = "The OK response")]
    public async Task<IActionResult> GetPlzAndSnowStatistics(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req,
        [DurableClient] IDurableClient durableClient
    )
    {
        var zipCodeValue = req.Query["zip_code"];
        var instanceId = await durableClient.StartNewAsync("PlzAndSnowStatisticsOrchestration", new QueryDto
        {
            Plz = zipCodeValue
        });

        _logger.LogInformation($"¨Started orchestration with id {instanceId}");

        return durableClient.CreateCheckStatusResponse(req, instanceId);
    }

    /// <summary>
    ///     Durable function, calling zip code and snow data endpoint public url,
    ///     getting snow data
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [FunctionName("PlzAndSnowStatisticsOrchestration")]
    public async Task<IEnumerable<SnowStatisticsDto>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var queryDto = context.GetInput<QueryDto>();
        var cities = await RetrieveCities(context, queryDto.Plz);

        if (cities == null) return null;

        var distinctCitiesNames = cities.Select(x => Regex.Match(x.Name, @"^([\w\-]+)").Value).Distinct().ToArray();
        var snowStatistics = await RetrieveSnowStatistics(context, distinctCitiesNames);

        return snowStatistics;
    }

    /// <summary>
    ///     Retrieving cities referenced by zip code (plz)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="plz"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<IEnumerable<CityDto>> RetrieveCities(IDurableOrchestrationContext context, string plz)
    {
        var plzFunctionEndpoint = Environment.GetEnvironmentVariable("PlzFunctionEndpoint");
        if (plzFunctionEndpoint == null)
            throw new InvalidOperationException("Plz function endpoint string can't be null!");

        var plzFunctionUri = UriWithZipCode(plz, plzFunctionEndpoint);
        _logger.LogInformation($"Retrieving cities referenced by plz {plz}");

        var plzResult = await context.CallHttpAsync(HttpMethod.Get, plzFunctionUri);

        return plzResult.StatusCode != HttpStatusCode.OK
            ? null
            : JsonConvert.DeserializeObject<IEnumerable<CityDto>>(plzResult.Content);
    }

    /// <summary>
    /// Since we need to pass code for azure function
    /// we can't just append query parameter zip_code to url
    /// </summary>
    /// <param name="plz"></param>
    /// <param name="plzFunctionEndpoint"></param>
    /// <returns></returns>
    private static Uri UriWithZipCode(string plz, string plzFunctionEndpoint)
    {
        var uriBuilder = new UriBuilder(plzFunctionEndpoint);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["zip_code"] = plz;
        uriBuilder.Query = query.ToString() ?? string.Empty;
        return uriBuilder.Uri;
    }

    /// <summary>
    ///     Retrieving snow statistics referenced
    ///     by city names
    /// </summary>
    /// <param name="context"></param>
    /// <param name="citiesNames"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<IEnumerable<SnowStatisticsDto>> RetrieveSnowStatistics(IDurableOrchestrationContext context,
        IEnumerable<string> citiesNames)
    {
        var snowStatisticsEndpoint = Environment.GetEnvironmentVariable("SnowStatisticsEndpoint");
        if (snowStatisticsEndpoint == null)
            throw new InvalidOperationException("Snow statistics endpoint string can't be null!");

        var getSnowDataDto = new GetSnowDataDto
        {
            CitiesNames = citiesNames
        };

        var snowStatisticsResult = await context.CallHttpAsync(HttpMethod.Post, new Uri(snowStatisticsEndpoint),
            JsonConvert.SerializeObject(getSnowDataDto));

        return snowStatisticsResult.StatusCode != HttpStatusCode.OK
            ? null
            : JsonConvert.DeserializeObject<IEnumerable<SnowStatisticsDto>>(snowStatisticsResult.Content);
    }
}