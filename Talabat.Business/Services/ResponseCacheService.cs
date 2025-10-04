using Talabat.Core.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.Business.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database= redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null) return;

            var options=new JsonSerializerOptions(){ PropertyNamingPolicy=JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(response,options);
            await _database.StringSetAsync(key, serializedResponse, timeToLive);
        }

        public async Task<string> GetCacheResponseAsync(string key)
        {
            var response= await _database.StringGetAsync(key);
            if (response.IsNullOrEmpty) return null;
            return response;
        }
    }
}
