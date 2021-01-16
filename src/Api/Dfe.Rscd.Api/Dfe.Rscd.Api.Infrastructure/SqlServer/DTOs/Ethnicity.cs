using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class Ethnicity
    {
        internal string EthnicityCode { get; set; }
        internal string EthnicityDescription { get; set; }
        internal string ParentEthnicityCode { get; set; }
    }
}
