using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class Ethnicity
    {
        public string EthnicityCode { get; set; }
        public string EthnicityDescription { get; set; }
        public string ParentEthnicityCode { get; set; }
    }
}
