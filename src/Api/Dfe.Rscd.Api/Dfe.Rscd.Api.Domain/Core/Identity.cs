using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Core
{
    public abstract class Identity<T> : IIdentity
    {
        private static readonly string NameWithDash;
        private static readonly Regex ValueValidation;


        static Identity()
        {
            var nameReplace = new Regex("id$");
            NameWithDash = nameReplace.Replace(typeof(T).Name, string.Empty).ToLowerInvariant() + "-";
            ValueValidation =
                new Regex(@"^[^\-]+\-(?<guid>[a-f0-9]{8}\-[a-f0-9]{4}\-[a-f0-9]{4}\-[a-f0-9]{4}\-[a-f0-9]{12})$",
                    RegexOptions.Compiled);
        }

        public Identity()
        {
        }

        protected Identity(string value)
        {
            var validationErrors = Validate(value).ToList();
            if (validationErrors.Any())
            {
                //throw new ArgumentException($"Identity is invalid: {string.Join(" ", validationErrors)}");
            }

            Value = value;
        }

        public static T New => With(Guid.NewGuid());

        public string Value { get; set; }

        public static T With(Guid guid)
        {
            var value = $"{NameWithDash}{guid:D}";
            return With(value);
        }

        public static T With(string value)
        {
            try
            {
                return (T) Activator.CreateInstance(typeof(T), value);
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException != null) throw e.InnerException;

                throw;
            }
        }

        public static IEnumerable<string> Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) yield return $"Identity of type '{typeof(T).Name}' is null or empty.";

            if (!string.Equals(value.Trim(), value, StringComparison.OrdinalIgnoreCase))
                yield return $"Identity '{value}' of type '{typeof(T).Name}' contains leading and/or trailing spaces.";
            if (!value.StartsWith(NameWithDash))
                yield return $"Identity '{value}' of type '{typeof(T).Name}' does not start with {NameWithDash}.";
            if (!ValueValidation.IsMatch(value))
                yield return
                    $"Identity '{value}' of type '{typeof(T).Name}' does not follow the syntax '[NAME]-[GUID]' in lower case.";
        }
    }
}