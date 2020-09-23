using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IPupilService
    {
        Pupil GetById(string checkingWindow, string id);
        Pupil GetById(string checkingWindow, PupilId id);
        List<Pupil> GetByUrn(string checkingWindow, URN urn); 
    }
}
