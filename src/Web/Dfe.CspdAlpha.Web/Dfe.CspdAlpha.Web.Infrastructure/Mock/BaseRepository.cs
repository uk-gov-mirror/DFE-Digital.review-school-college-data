using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Dfe.CspdAlpha.Web.Infrastructure.Mock
{
    public class BaseRepository
    {
        private class ResourceData<T>
        {
            public List<T> data { get; set; }
        }

        protected List<T> GetReourceData<T>(string resourceData)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resultJson = string.Empty;
            using (var stream = assembly.GetManifestResourceStream(resourceData))
            using (var reader = new StreamReader(stream))
            {
                resultJson = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<ResourceData<T>>(resultJson).data;
        }
    }
}
