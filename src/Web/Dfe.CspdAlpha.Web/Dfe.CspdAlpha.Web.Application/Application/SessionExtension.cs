using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Dfe.Rscd.Web.Application.Application
{
    public static class SessionExtension
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static void Set<T>(this ISession session, string key, T value)
        { 
            session.SetString(key, JsonConvert.SerializeObject(value, Formatting.Indented, settings));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
