using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;

namespace Dfe.CspdAlpha.Web.Application.Models.School
{
    public class Amendment
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PupilId { get; set; }
        public DateTime DateRequested { get; set; }
        public string ReferenceId { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
    }
}
