using Dfe.Rscd.Api.Domain.Entities;
using System;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces
{
    public interface IAmendmentBuilder
    {
        Guid BuildAmendments(Amendment amendment);
    }
}
