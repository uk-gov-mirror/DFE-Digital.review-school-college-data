using System.Collections.Generic;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Config
{
    public class CosmosDbOptions
    {
        public string Account { get; set; }

        public string Key { get; set; }

        public string Database { get; set; }

        public IDictionary<string, string> CollectionLookup { get; set; }
    }
}