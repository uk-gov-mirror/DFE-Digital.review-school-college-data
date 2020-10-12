using System;
using System.Collections.Generic;
using System.Text;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupil
    {
        public AddReason Reason { get; set; }
        public List<PriorAttainment> PriorAttainmentResults { get; set; }
    }
}
