using System;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces
{
    public interface IAmendmentBuilder
    {
        Guid BuildAmendments(IAmendment amendment);
        AmendmentType AmendmentType { get; }
    }
}