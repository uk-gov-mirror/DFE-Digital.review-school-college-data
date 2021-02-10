using System;
using System.Globalization;

namespace Dfe.Rscd.Api.Domain.Common
{
    public static class ExtensionMethods
    {
        public static DateTime ToDateTimeWhenSureNotNull(this string potentialDateString)
        {
            var date = potentialDateString.ToDateTime();
            if (date.HasValue)
                return date.Value;

            return DateTime.MinValue;
        }

        public static DateTime? ToDateTime(this string potentialDateString)
        {
            if (DateTime.TryParseExact(potentialDateString, "d-M-yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out var newDate))
            {
                return newDate;
            }

            if (DateTime.TryParseExact(potentialDateString, "dd-MM-yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out newDate))
            {
                return newDate;
            }

            if (DateTime.TryParseExact(potentialDateString, "d/M/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out newDate))
            {
                return newDate;
            }

            if (DateTime.TryParseExact(potentialDateString, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out newDate))
            {
                return newDate;
            }

            return null;
        }
    }
}