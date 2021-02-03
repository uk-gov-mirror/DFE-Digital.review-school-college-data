using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IPupilService
    {
        Pupil GetById(CheckingWindow checkingWindow, string id);
        List<PupilRecord> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query);
    }
}