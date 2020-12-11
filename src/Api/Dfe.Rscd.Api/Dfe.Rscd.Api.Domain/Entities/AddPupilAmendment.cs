using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AddPupilAmendment : Amendment
    {
        public AddPupilAmendment()
        {
            AmendmentType = AmendmentType.AddPupil;
        }
    }
}