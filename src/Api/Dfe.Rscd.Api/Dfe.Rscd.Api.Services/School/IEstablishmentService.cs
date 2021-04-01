﻿using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IEstablishmentService
    {
        School GetByURN(CheckingWindow checkingWindow, URN urn);
        School GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber);
        bool DoesSchoolExist(string dfesNumber);
        bool IsNonPlascEstablishment(CheckingWindow checkingWindow, URN urn);
    }
}