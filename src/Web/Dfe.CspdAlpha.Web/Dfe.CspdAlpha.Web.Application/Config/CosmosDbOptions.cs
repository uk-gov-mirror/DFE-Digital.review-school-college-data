using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.CspdAlpha.Web.Application.Config
{
    public class CosmosDbOptions
    {
        public string Account { get; set; }

        public string Key { get; set; }

        public string Database { get; set; }
        public string EstablishmentsCollection { get; set; }
        public string PupilsCollection { get; set; }

    }
}
