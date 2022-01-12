using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace SwissSnowApp.Common.CacheManager;

/// <summary>
///     PLZ Redis cache Manager implementation
/// </summary>
public class CacheManager : ICacheManager
{
    private IDatabaseAsync _dbCache;

    private IDatabaseAsync DbCache
    {
        get
        {
            if (_dbCache != null) return _dbCache;

            var connectionString = Environment.GetEnvironmentVariable("CacheConnectionString");
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _dbCache = connection.GetDatabase();

            return _dbCache;
        }
    }

    /// <summary>
    ///     Getting object of type T referenced by string key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>?> Get<T>(string key)
    {
        var redisResult = await DbCache.StringGetAsync(new RedisKey(key));
        return !redisResult.HasValue ? null : JsonConvert.DeserializeObject<IEnumerable<T>>(redisResult.ToString());
    }


    /// <summary>
    ///     Inserting / updating objects of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public async Task InsertOrUpdate<T>(string key, IEnumerable<T> objects)
    {
        var serializedCities = JsonConvert.SerializeObject(objects);
        await DbCache.StringSetAsync(key, serializedCities, new TimeSpan(0, 1, 0, 0));
    }
}