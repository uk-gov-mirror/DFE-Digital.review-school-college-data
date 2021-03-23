using System;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using StackExchange.Redis;
using ITransientErrorDetectionStrategy = Microsoft.Rest.TransientFaultHandling.ITransientErrorDetectionStrategy;
using RetryPolicy = Microsoft.Rest.TransientFaultHandling.RetryPolicy;

namespace Dfe.Rscd.Web.Application.Application.Cache.Redis
{
    using L = Lazy<ConnectionMultiplexer>;

    public interface IRedisCache
    {
        T Get<T>(string key, int databaseId = -1);
        T GetOrCreate<T>(string key, Func<T> action, TimeSpan? expiry = null, Action<string> onCacheItemCreation = null, int databaseId = -1);
        void Remove(string key, int databaseId = -1);
        void Remove(string[] keys, int databaseId = -1);

        void Clear(int databaseId = -1);
    }

    public class RedisCache : IRedisCache
    {
        private static readonly L Connection = new L(() =>
        {
            var options = ConfigurationOptions.Parse(Program.RedisConnectionString);
            options.AllowAdmin = true;
            return ConnectionMultiplexer.Connect(options);

        }, LazyThreadSafetyMode.PublicationOnly);

        private class RedisCacheTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
        {
            /// <inheritdoc />
            public bool IsTransient(Exception ex)
            {
                switch (ex)
                {
                    case null:
                        return false;
                    case TimeoutException _:
                    case RedisServerException _:
                    case RedisException _:
                        return true;
                }

                return ex.InnerException != null && IsTransient(ex.InnerException);
            }
        }

        private readonly RetryPolicy _retryPolicy;
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisCache() : this(Connection.Value)
        {
        }

        public RedisCache(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _retryPolicy =
                new Microsoft.Rest.TransientFaultHandling.RetryPolicy<RedisCacheTransientErrorDetectionStrategy>(3,
                    TimeSpan.FromSeconds(2));
        }

        public T GetOrCreate<T>(string key, Func<T> action, TimeSpan? expiry = null,
            Action<string> onCacheItemCreation = null, int databaseId = -1)
        {
            var db = _connectionMultiplexer.GetDatabase(databaseId);
            var redisResult = _retryPolicy.ExecuteAction(() => db.StringGet(key));

            if (redisResult.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(redisResult);
            }

            var result = action();

            if (result != null)
            {
                _retryPolicy.ExecuteAction(() => db.StringSet(key, JsonConvert.SerializeObject(result), expiry));
                onCacheItemCreation?.Invoke(key);
            }

            return result;
        }

        public void Remove(string key, int databaseId = -1) =>
            _connectionMultiplexer.GetDatabase(databaseId).KeyDelete(key);

        public void Clear(int databaseId = -1)
        {
            var endpoints = _connectionMultiplexer.GetEndPoints();
            var server = _connectionMultiplexer.GetServer(endpoints.First());
            var keys = server.Keys();
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public void Remove(string[] keys, int databaseId = -1) => _connectionMultiplexer.GetDatabase(databaseId)
            .KeyDelete(keys.Select(x => (RedisKey) x).ToArray());

        public T Get<T>(string key, int databaseId = -1)
        {
            var redisResult =
                _retryPolicy.ExecuteAction(() => _connectionMultiplexer.GetDatabase(databaseId).StringGet(key));
            return !redisResult.HasValue
                ? (default)
                : typeof(T) == typeof(string)
                    ? (T) Convert.ChangeType(redisResult, typeof(T))
                    : JsonConvert.DeserializeObject<T>(redisResult);
        }
    }
}
