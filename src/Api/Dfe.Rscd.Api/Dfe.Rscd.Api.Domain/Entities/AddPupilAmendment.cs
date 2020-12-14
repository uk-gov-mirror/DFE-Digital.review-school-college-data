using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupilAmendment : Amendment
    {
        public const string FIELD_Reason = "Reason";
        public const string FIELD_PreviousSchoolURN = "PreviousSchoolURN";
        public const string FIELD_PreviousSchoolLAEstab = "PreviousSchoolLAEstab";
        public const string FIELD_PriorAttainmentResults = "PriorAttainmentResults";

        public AddPupilAmendment()
        {
            AmendmentType = AmendmentType.AddPupil;

            AmendmentDetail.AddField(FIELD_Reason, new AddReason());
            AmendmentDetail.AddField(FIELD_PreviousSchoolURN, string.Empty);
            AmendmentDetail.AddField(FIELD_PreviousSchoolLAEstab, string.Empty);
            AmendmentDetail.AddField(FIELD_PriorAttainmentResults, new List<PriorAttainment>());
        }
    }
}