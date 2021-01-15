using System;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core
{
    public class URN
    {
        public URN()
        {
        }

        public URN(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn)) throw new ApplicationException("Empty of null urn provided");
            if (urn.Length != 6 || !int.TryParse(urn, out var urnNumber))
                throw new ApplicationException($"URN in invalid format: {urn}");

            Value = urn;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}