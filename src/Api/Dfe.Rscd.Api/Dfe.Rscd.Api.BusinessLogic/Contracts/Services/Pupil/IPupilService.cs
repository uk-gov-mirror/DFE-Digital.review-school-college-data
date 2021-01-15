using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services
{
    public interface IPupilService
    {
        Pupil GetById(CheckingWindow checkingWindow, string id);
        List<PupilRecord> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query);
    }
}