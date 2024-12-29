using Application.Abstractions.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Caching;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _redisDatabase;

    public CacheService(IConnectionMultiplexer redisConnection)
    {
        _redisDatabase = redisConnection.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
       
        var jsonData = JsonSerializer.Serialize(value);
        await _redisDatabase.StringSetAsync(key, jsonData, expiration);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await _redisDatabase.StringGetAsync(key);

        if (jsonData.IsNullOrEmpty)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveAsync(string key)
    {
        await _redisDatabase.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _redisDatabase.KeyExistsAsync(key);
    }
}
