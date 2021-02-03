using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class QuestionViewModel : ContextAwareViewModel
    {
        public string GetTitle()
        {
            return CurrentQuestion.Title;
        }

        public PupilViewModel PupilDetails { get; set; }
        public string PupilLabel => GetPupilLabel();

        public ICollection<Question> Questions { get;set; }

        public int CurrentQuestionIndex { get; set; }

        public bool HasMoreQuestions => Questions.Count > CurrentQuestionIndex;

        private string GetPupilLabel()
        {
            return PersonLowercase;
        }

        public Question CurrentQuestion => Questions.ToList()[CurrentQuestionIndex];

        public string BackController { get; set; }
        public string BackAction { get; set; }

        public QuestionViewModel(ICollection<Question> questions, int currentIndex=0)
        {
            Questions = questions;
            CurrentQuestionIndex = currentIndex;
        }
    }
}
