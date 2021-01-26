using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class PromptViewModel : ContextAwareViewModel
    {
        public string GetTitle()
        {
            return CurrentQuestion.PromptShortText;
        }

        public PupilViewModel PupilDetails { get; set; }
        public string PupilLabel => GetPupilLabel();

        public List<Prompt> Questions { get;set; }

        public int CurrentQuestionIndex { get; set; }

        public bool HasMoreQuestions => Questions.Count > CurrentQuestionIndex;

        private string GetPupilLabel()
        {
            return PersonLowercase;
        }

        public Prompt CurrentQuestion
        {
            get
            {
                return Questions[CurrentQuestionIndex];
            }
        }

        public string BackController { get; set; }
        public string BackAction { get; set; }

        public PromptViewModel(List<Prompt> prompts, int currentIndex=0)
        {
            Questions = prompts;
            CurrentQuestionIndex = currentIndex;
        }
    }
}
