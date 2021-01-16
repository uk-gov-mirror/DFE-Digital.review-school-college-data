
using Web09.Checking.Business.Logic.Entities;
using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractPupilAdjustmentTypeAndBusinessEntityStudentAdjustmentType
    {
        public static StudentAdjustmentType TranslanteDatacontractPupilAdjustmentTypeToBusinessEntityStudentAdjustmentType(PupilAdjustmentType adjustmentTypeIn)
        {
            StudentAdjustmentType adjustmentTypeOut = new StudentAdjustmentType();

            switch (adjustmentTypeIn)
            {
                case(PupilAdjustmentType.InclusionAdjustmentForPupilAdd):
                    adjustmentTypeOut = StudentAdjustmentType.InclusionAdjustmentForPupilAdd;
                    break;
                case(PupilAdjustmentType.InclusionAdjustmentForPupilEdit):
                    adjustmentTypeOut = StudentAdjustmentType.InclusionAdjustmentForPupilEdit;
                    break;
                case(PupilAdjustmentType.InclusionAdjustmentForPupilMove):
                    adjustmentTypeOut = StudentAdjustmentType.InclusionAdjustmentForPupilMove;
                    break;
                case(PupilAdjustmentType.PupilInclusionAdjustmentOnly):
                    adjustmentTypeOut = StudentAdjustmentType.PupilInclusionAdjustmentOnly;
                    break;
                case (PupilAdjustmentType.PupilRemovalAdjustmentOnly):
                    adjustmentTypeOut = StudentAdjustmentType.PupilRemovalAdjustmentOnly;
                    break;
                case (PupilAdjustmentType.RemovalAdjustmentForPupilEdit):
                    adjustmentTypeOut = StudentAdjustmentType.RemovalAdjustmentForPupilEdit;
                    break;
            }

            return adjustmentTypeOut;
        }
    }
}
