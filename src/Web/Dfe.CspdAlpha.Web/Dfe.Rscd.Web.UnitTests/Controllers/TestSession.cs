using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public class TestSession : ISession
    {
        public IDictionary<string, object> _session = new ConcurrentDictionary<string, object>();

        public void Clear()
        {
            _session.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = new())
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = new())
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _session.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            if (!_session.ContainsKey(key))
                _session.Add(key, value);
            _session[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_session.Keys.Contains(key))
            {
                value = (byte[]) _session[key];
                return true;
            }

            value = null;
            return false;
        }

        public string Id { get; }
        public bool IsAvailable { get; }
        public IEnumerable<string> Keys { get; }
    }
}