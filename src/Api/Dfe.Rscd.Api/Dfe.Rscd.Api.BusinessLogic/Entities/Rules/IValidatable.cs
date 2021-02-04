using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public interface IValidatable
    {
        bool IsValid();
        List<string> Validate();
    }
}
