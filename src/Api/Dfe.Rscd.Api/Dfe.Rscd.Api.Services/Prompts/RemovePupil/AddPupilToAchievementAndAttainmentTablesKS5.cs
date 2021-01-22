using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public partial class RemovePupilPromptsService
    {
        private static AmendmentOutcome AddPupilToAchievementAndAttainmentTablesKS5(Pupil student, int inclusionReasonId)
        {
            //List<Prompts> promptListReturn = new List<Prompts>();

            //if (student == null || student.StudentChanges.Count == 0)
            //    throw Web09Exception.GetBusinessException(MessageList.InsufficientStudentDetails);

            //StudentChanges studentChange = student.StudentChanges.First();

            //if (studentChange == null || studentChange.YearGroups == null)
            //    throw Web09Exception.GetBusinessException(MessageList.InsufficientStudentDetails);

            //int yearGroup;
            //if (int.TryParse(studentChange.YearGroups.YearGroupCode, out yearGroup) && yearGroup == 13)
            //{
            //    //Return prompt 5710
            //    return new AmendmentOutcome(new CompletedStudentAdjustment(student.StudentID,
            //        inclusionReasonId,
            //        new PromptAnswerList(),
            //        Constants.SCRUTINY_REASON_ADD_PUPIL,
            //        null,
            //        Constants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
            //        GetInfoPromptText(5710))
            //        );
            //}
            //else
            //{
            //    //Return prompt 5720 and initialise prompt set for reason 52.
            //    return ProcessKS5NCYearGroupAdjustment(student, inclusionReasonId, new PromptAnswerList(), 5720);
            //}

            return new AmendmentOutcome(new CompletedNonStudentAdjustment("complete"));
        }
    }
}
