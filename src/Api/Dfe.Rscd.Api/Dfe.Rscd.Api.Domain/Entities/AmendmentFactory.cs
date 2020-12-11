using System;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentFactory
    {
        public static IAmendment CreateAmendment(AmendmentType type)
        {
            if (type == AmendmentType.AddPupil)
            {
                return new AddPupilAmendment();
            }
            
            if(type == AmendmentType.RemovePupil)
            {
                return new RemovePupilAmendment();
            }

            throw new NotImplementedException("AmendmentType not Found!");
        }
    }
}