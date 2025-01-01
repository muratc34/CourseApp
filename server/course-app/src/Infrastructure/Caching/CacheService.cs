using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace Infrastructure.Caching;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _redisDatabase;
    private readonly IConnectionMultiplexer _redisConnection;

    public CacheService(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
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

    public async Task RemoveAsync(string pattern)
    {
        var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
        var keys = server.Keys(pattern: pattern).ToList();
        foreach (var key in keys)
        {
            await _redisDatabase.KeyDeleteAsync(key);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _redisDatabase.KeyExistsAsync(key);
    }
}
