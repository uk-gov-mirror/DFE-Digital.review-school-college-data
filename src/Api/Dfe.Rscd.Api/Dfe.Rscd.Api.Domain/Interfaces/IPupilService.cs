using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IPupilService
    {
        Pupil GetById(CheckingWindow checkingWindow, string id);
        List<Pupil> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query);
    }
}
