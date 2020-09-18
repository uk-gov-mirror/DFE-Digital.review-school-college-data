using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IPupilService
    {
        Pupil GetById(string id);
        Pupil GetById(PupilId id);

        List<Pupil> GetByUrn(URN urn);

        List<Pupil> FindMatchedPupils(Pupil pupil);
    }
}
