using System.Text.Json;
using StackExchange.Redis;
using TraineeManagement.Api.Data.CacheServiceInterface;
using Microsoft.Extensions.Logging;

namespace TraineeManagement.Api.Data.CacheService;

public class CacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connection;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IConnectionMultiplexer connection, ILogger<CacheService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _logger.LogWarning("Dependency failure: Redis unavailable. Cache miss. CacheKey: {CacheKey}", key);
                return default;
            }

            RedisValue value = await _connection.GetDatabase().StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                _logger.LogDebug("Cache miss. CacheKey: {CacheKey}", key);
                return default;
            }

            _logger.LogDebug("Cache hit. CacheKey: {CacheKey}", key);
            return JsonSerializer.Deserialize<T>(value!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dependency failure: Cache GET failed. CacheKey: {CacheKey}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _logger.LogWarning("Dependency failure: Redis unavailable. Skipping cache SET. CacheKey: {CacheKey}", key);
                return;
            }

            string json = JsonSerializer.Serialize(value);
            await _connection.GetDatabase().StringSetAsync(key, json, ttl);

            _logger.LogDebug("Cache set. CacheKey: {CacheKey}, TTL: {TTL}", key, ttl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dependency failure: Cache SET failed. CacheKey: {CacheKey}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _logger.LogWarning("Dependency failure: Redis unavailable. Skipping cache REMOVE. CacheKey: {CacheKey}", key);
                return;
            }

            await _connection.GetDatabase().KeyDeleteAsync(key);
            _logger.LogDebug("Cache evict. CacheKey: {CacheKey}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dependency failure: Cache REMOVE failed. CacheKey: {CacheKey}", key);
        }
    }

    public async Task RemoveManyAsync(params string[] keys)
    {
        foreach (string key in keys)
        {
            await RemoveAsync(key);  
        }
    }
}