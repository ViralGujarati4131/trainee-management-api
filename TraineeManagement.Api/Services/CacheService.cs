using System.Text.Json;
using StackExchange.Redis;
using TraineeManagement.Api.CacheServiceInterface;

namespace TraineeManagement.Api.CacheService;

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
                _logger.LogWarning("Redis unavailable. Cache MISS for key: {Key}", key);
                return default;
            }

            RedisValue value = await _connection.GetDatabase().StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                _logger.LogDebug("Cache MISS for key: {Key}", key);
                return default;
            }

            _logger.LogDebug("Cache HIT for key: {Key}", key);
            return JsonSerializer.Deserialize<T>(value!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache GET failed for key: {Key}. Falling back to DB", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _logger.LogWarning("Redis unavailable. Skipping cache SET for key: {Key}", key);
                return;
            }

            string json = JsonSerializer.Serialize(value);
            await _connection.GetDatabase().StringSetAsync(key, json, ttl);

            _logger.LogDebug("Cache SET for key: {Key} with TTL: {TTL}", key, ttl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache SET failed for key: {Key}. Continuing without cache", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _logger.LogWarning("Redis unavailable. Skipping cache REMOVE for key: {Key}", key);
                return;
            }

            await _connection.GetDatabase().KeyDeleteAsync(key);
            _logger.LogDebug("Cache INVALIDATED for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache REMOVE failed for key: {Key}", key);
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