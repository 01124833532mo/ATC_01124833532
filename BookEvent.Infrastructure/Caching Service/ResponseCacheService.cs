using BookEvent.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using StackExchange.Redis;
using System.Text.Json;

namespace BookEvent.Infrastructure.Caching_Service
{
    public class ResponseCacheService(IConnectionMultiplexer connectionMultiplexer) : IResponseCacheService
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await _database.StringSetAsync(key, serializedResponse, timeToLive);

        }

        public async Task<string?> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _database.StringGetAsync(key);
            if (cachedResponse.IsNullOrEmpty)
            {
                return null!;
            }
            return cachedResponse;
        }
    }

}
