using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IPupilService
    {
        Pupil GetById(PupilId id);

        List<Pupil> GetByUrn(URN urn);

        List<Pupil> FindMatchedPupils(Pupil pupil);
    }
}
