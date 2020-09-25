using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Dfe.CspdAlpha.Admin
{
    public class NumberJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        private string[] conversionWhiteList = new[] {"URN", "DFESNumber", "TELNUM", "SchoolID", "CandidateNumber", "UPN", "ULN"};

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.String || conversionWhiteList.Any(w => writer.Path.Contains(w)))
            {
                t.WriteTo(writer);
            }
            else
            {
                if (int.TryParse(value.ToString(), out var intResult))
                {
                    t = JToken.FromObject(intResult);
                }
                else if (decimal.TryParse(value.ToString(), out var decResult))
                {
                    t = JToken.FromObject(decResult);
                }

                t.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException(
                "Unnecessary because CanRead is false. The type will skip the converter.");
        }
    }
}
