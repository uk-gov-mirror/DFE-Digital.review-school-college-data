using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Dfe.Rscd.Web.Application.Application.Cache.Memory
{
    public interface IInMemoryCache
    {
        T Get<T>(string key);
        T GetOrSet<T>(string key, Func<T> factory, DateTimeOffset absoluteExpiration);
        T GetOrSet<T>(string key, Func<T> factory, TimeSpan slidingExpiration);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, DateTimeOffset absoluteExpiration);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan slidingExpiration);
        void Remove(string key);
        void Remove(string[] keys);
    }

    public class InMemoryCache : IInMemoryCache
    {
        public void Remove(string key) => MemoryCache.Default.Remove(key);

        public T Get<T>(string key)
        {
            var obj = MemoryCache.Default.Get(key);
            if (obj != null && obj is T)
            {
                return (T) obj;
            }
            else
            {
                return default;
            }
        }
        public T GetOrSet<T>(string key, Func<T> factory, TimeSpan slidingExpiration) => GetOrSet(key, factory, slidingExpiration, DateTimeOffset.MaxValue);

        public T GetOrSet<T>(string key, Func<T> factory, DateTimeOffset absoluteExpiration) => GetOrSet(key, factory, TimeSpan.Zero, absoluteExpiration);

        private T GetOrSet<T>(string key, Func<T> factory, TimeSpan slidingExpiration, DateTimeOffset absoluteExpiration)
        {
            if (!(MemoryCache.Default.Get(key) is T item))
            {
                item = factory();
                MemoryCache.Default.Add(key, item, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration, SlidingExpiration = slidingExpiration });
            }
            return item;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan slidingExpiration) => await GetOrSetAsync(key, factory, slidingExpiration, DateTimeOffset.MaxValue).ConfigureAwait(false);

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, DateTimeOffset absoluteExpiration) => await GetOrSetAsync(key, factory, TimeSpan.Zero, absoluteExpiration).ConfigureAwait(false);

        private async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan slidingExpiration, DateTimeOffset absoluteExpiration)
        {
            if (!(MemoryCache.Default.Get(key) is T item))
            {
                item = await factory().ConfigureAwait(false);
                MemoryCache.Default.Add(key, item, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration, SlidingExpiration = slidingExpiration });
            }
            return item;
        }

        public void Remove(string[] keys)
        {
            foreach (var key in keys ?? new string[0])
            {
                Remove(key);
            }
        }
    }
}
