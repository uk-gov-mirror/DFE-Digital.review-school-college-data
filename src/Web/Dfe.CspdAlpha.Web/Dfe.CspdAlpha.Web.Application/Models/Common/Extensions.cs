using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Dfe.Rscd.Web.Application.Models.Common
{
    public static class Extensions
    {
        public static string Encode(this string unencoded)
        {
            var replacementChars = new Dictionary<string, string> {{"Ã§","ç"}};
            var newString = string.Empty;
            foreach(var c in replacementChars)
            {
                newString = unencoded.Replace(c.Key, c.Value);
            }

            return newString;
        }
    }
}
