using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwissSnowApp.Common.CacheManager
{
    public interface ICacheManager
    {
        public Task<IEnumerable<T>?> Get<T>(string key);
        public Task InsertOrUpdate<T>(string key, IEnumerable<T> objects);
    }
}
