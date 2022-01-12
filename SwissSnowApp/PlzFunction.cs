using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SwissSnowApp.Interfaces;

namespace SwissSnowApp
{
    public class PlzFunction
    {
        private readonly ILogger<PlzFunction> _logger;
        private readonly IZipCodeRetriever _zipCodeRetriever;

        public PlzFunction(IZipCodeRetriever zipCodeRetriever, ILogger<PlzFunction> logger)
        {
            _logger = logger;
            _zipCodeRetriever = zipCodeRetriever;
        }

        [FunctionName("PlzFunction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Azure function started, triggered by HTTP trigger GET.");

            var zipCodeValue = req.Query["zip_code"];
            var result = await _zipCodeRetriever.RetrieveZipCode(zipCodeValue);

            if (!result.Any())
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }
    }
}

