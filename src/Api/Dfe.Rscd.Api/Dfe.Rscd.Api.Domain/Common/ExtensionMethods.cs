using System;
using System.Globalization;

namespace Dfe.Rscd.Api.Domain.Common
{
    public static class ExtensionMethods
    {
        public static DateTime ToDateTimeWhenSureNotNull(this string potentialDateString, string format)
        {
            if (DateTime.TryParseExact(potentialDateString, format, new CultureInfo("en-GB"), DateTimeStyles.None, out var newDate))
            {
                return newDate;
            }

            return DateTime.MinValue;
        }

        public static DateTime? ToDateTime(this string potentialDateString, string format)
        {
            if (DateTime.TryParseExact(potentialDateString, format, new CultureInfo("en-GB"), DateTimeStyles.None, out var newDate))
            {
                return newDate;
            }

            return null;
        }
    }
}