using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels
{
    public abstract class ContextAwareViewModel
    {
        private readonly string _phase;

        protected ContextAwareViewModel()
        {
            _phase = Context.Current.Request.RouteValues["phase"].ToString();
        }

        protected ContextAwareViewModel(string phase)
        {
            _phase = phase;
        }

        public CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(_phase);

        public string PersonLowercase => CheckingWindow == CheckingWindow.KS5 ? "student" : "pupil";
        public string PersonTitlecase => CheckingWindow == CheckingWindow.KS5 ? "Student" : "Pupil";
        public string PersonIdAcronym => CheckingWindow == CheckingWindow.KS5 ? "ULN" : "UPN";

        public string PersonIdLabel => CheckingWindow == CheckingWindow.KS5 ? "ULN (Unique Learner Number)" : "UPN (Unique Pupil Number)";

    }
}
