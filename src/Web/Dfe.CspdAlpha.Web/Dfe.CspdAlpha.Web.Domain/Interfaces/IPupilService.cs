using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IPupilService
    {
        Pupil GetById(string checkingWindow, string id);
        List<Pupil> QueryPupils(string checkingWindow, PupilQuery query); 
    }
}
