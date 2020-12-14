using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentDetail
    {
        public AmendmentDetail()
        {
            Fields = new List<AmendmentField>();
        }

        public IList<AmendmentField> Fields { get; }

        public void AddField(string name, object value)
        {
            var field = Fields.SingleOrDefault(x => x.Name == name);
            if (field == null)
                Fields.Add(new AmendmentField {Name = name, Value = value});
            else
                field.Value = value;
        }

        public void SetField(string name, object value)
        {
            var field = Fields.SingleOrDefault(x => x.Name == name);
            if (field != null) field.Value = value;
        }

        public object GetField(string name)
        {
            var field = Fields.SingleOrDefault(x => x.Name == name);
            return field?.Value;
        }

        public T GetField<T>(string name)
        {
            return (T) GetField(name);
        }
    }
}