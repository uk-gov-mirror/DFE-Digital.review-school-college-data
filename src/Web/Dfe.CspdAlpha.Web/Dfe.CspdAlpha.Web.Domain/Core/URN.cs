using System;

namespace Dfe.CspdAlpha.Web.Domain.Core
{
    public class URN
    { 
        public string Value { get; set; }

        public URN()
        {
            
        }

        public URN(string urn)
        {
            if (string.IsNullOrWhiteSpace(urn))
            {
                throw new ApplicationException("Empty of null urn provided");
            }
            if(urn.Length != 6 || !int.TryParse(urn, out var urnNumber))
            {
                throw new ApplicationException($"URN in invalid format: {urn}");
            }

            Value = urn;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
