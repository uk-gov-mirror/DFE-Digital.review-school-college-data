using System;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Amendments
{
    public class ReceivedViewModel : ContextAwareViewModel
    {
        public Guid NewAmendmentId { get; set; }
        public string NewAmendmentRef { get;set; }
    }
}
