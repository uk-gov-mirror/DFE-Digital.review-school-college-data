using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Dfe.Rscd.Web.ApiClient
{
    partial class AmendmentDetail
    {
        public AmendmentDetail()
        {
            Fields = new List<AmendmentField>();
        }

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

        public List<T> GetList<T>(string name)
        {
            var value = GetField(name);

            if (value != null && value.GetType() == typeof(JArray))
            {
                var jsonResponse = (JArray) value;
                var newList = new List<T>();

                foreach (var item in jsonResponse)
                {
                    var itemDeserialized = item.ToObject<T>();
                    newList.Add(itemDeserialized);
                }

                return newList;
            }

            return (List<T>) value;
        }

        public T GetField<T>(string name)
        {
            var stringRep = string.Empty + GetField(name);
            if (stringRep != string.Empty) return Convert<T>(stringRep);

            return default;
        }

        public static T Convert<T>(string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T) converter.ConvertFromString(input);
                return default;
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }
    }
}