using System;
using System.Globalization;

namespace Dfe.Rscd.Api.Domain.Common
{
    public static class ExtensionMethods
    {
        public static DateTime ToDateTimeWhenSureNotNull(this string potentialDateString, string format)
        {
            var date = potentialDateString.ToDateTime(format);
            if(date.HasValue)
                return date.Value;

            return DateTime.MinValue;
        }

        public static DateTime? ToDateTime(this string potentialDateString, string format)
        {
            var parts = potentialDateString.Split("/");
            if (parts.Length < 2)
                parts = potentialDateString.Split("-");

            try
            {
                var day = int.Parse(parts[0]);
                var month = int.Parse(parts[1]);
                var year = int.Parse(parts[2]);

                return new DateTime(year, month, day);
            }
            catch (Exception e)
            {
                if (DateTime.TryParseExact(potentialDateString, format, new CultureInfo("en-GB"), DateTimeStyles.None, out var newDate))
                {
                    return newDate;
                }
            }
      
            return null;
        }
    }
}