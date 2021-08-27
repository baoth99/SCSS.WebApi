using SCSS.Utilities.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.Interfaces
{
    public interface ICacheService
    {
        Task SetCacheData(CacheRedisKey key, string data);

        Task SetCacheData(CacheRedisKey key, byte[] data);

        Task<string> GetCacheData(CacheRedisKey key);

        Task<byte[]> GetCacheByteData(CacheRedisKey key);

        Task RemoveCacheData(CacheRedisKey key);
    }
}
