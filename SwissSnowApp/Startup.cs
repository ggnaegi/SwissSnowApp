using System.Reflection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SwissSnowApp;
using SwissSnowApp.Common.CacheManager;
using SwissSnowApp.Interfaces;
using SwissSnowApp.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SwissSnowApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<ICacheManager, CacheManager>();
            builder.Services.AddScoped<IZipCodeRetriever, ZipCodeRetriever>();
            builder.Services.AddScoped<IHttpClientManager, HttpClientManager>();
        }
    }
}
