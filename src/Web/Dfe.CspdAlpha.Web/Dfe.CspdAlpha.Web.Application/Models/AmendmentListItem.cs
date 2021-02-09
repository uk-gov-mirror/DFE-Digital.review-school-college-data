using System;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels;

namespace Dfe.Rscd.Web.Application.Models
{
    public class AmendmentListItem : ContextAwareViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Upn { get; set; }

        public string Uln { get; set; }
        public DateTime DateRequested { get; set; }
        public string ReferenceId { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public EvidenceStatus EvidenceStatus { get; set; }
    }
}
