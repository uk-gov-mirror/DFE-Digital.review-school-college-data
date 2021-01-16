using System;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent
    {

        public static AdjustmentPromptAnalysis ProcessStudentEdit(Students student)
        {
            //TODO: Throw error if P_INCL is unobtainable.

            switch (student.PINCLs.P_INCL)
            {
                case ("201"):
                    return ProcessStudentKS2NCYearGroupChange(student);
                default:
                    return null;
                    
            }

        }

        private static AdjustmentPromptAnalysis ProcessStudentKS2NCYearGroupChange(Students student)
        {
            //TODO: Validate if NC Year group is obtainable and that it's an integer value.

            Students studentPreEdit = GetStudent(student.StudentID);
            StudentChanges studentChangePreEdit = studentPreEdit.StudentChanges.First();

            StudentChanges studentChangePostEdit = student.StudentChanges.First();

            if (studentChangePreEdit.DOB != studentChangePostEdit.DOB)
            {
                PromptAnswer ncYearGroupAns = new PromptAnswer(Contants.PROMPT_ID_NC_YEAR_GROUP_KS2);
                ncYearGroupAns.PromptAnswerType = PromptAnswer.PromptAnswerTypeEnum.Integer;
                ncYearGroupAns.PromptIntegerAnswer = Convert.ToInt32(studentChangePostEdit.YearGroups.YearGroupCode);

                PromptAnswerList answerList = new PromptAnswerList();
                answerList.Add(ncYearGroupAns);

                return TSIncludeRemovePupil.ProcessKS2NCYearGroupAdjustment(student, 214, answerList, null);

            }
            else
            {
                return null;
            }
        }

    }
}
