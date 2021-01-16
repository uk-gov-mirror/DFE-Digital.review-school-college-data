using System;

namespace Dfe.Rscd.Api.BusinessLogic.Common
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Takes a DB string representation of a date (yyyymmdd) and returns a string representation of the date
        /// in the format dd/mm/yyyy
        /// </summary>
        /// <param name="value">DB representation of the date</param>
        /// <returns>Short format string representation of the date</returns>
        public static string ToShortDateTimeString(this string value)
        {
            if(string.IsNullOrEmpty(value))
                return String.Empty;

            if (value.Length != 8)            
                return String.Empty;            

            return String.Format(
                "{0}/{1}/{2}",
                value.Substring(6, 2),
                value.Substring(4, 2),
                value.Substring(0, 4));
        }

        /// <summary>
        /// Takes a short format string representation of a date (dd/mm/yyyy) and returns a DB formatted 
        /// string representation of the date (yyyymmdd)
        /// </summary>
        /// <param name="value">Short format string representation of the date</param>
        /// <returns>DB format string representation of the date</returns>
        public static string ToDBDateTimeString(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return String.Empty;

            if (value.Length != 10)            
                return String.Empty;            

            return String.Concat(
                value.Substring(6, 4),
                value.Substring(3, 2),
                value.Substring(0, 2));
        }

     
    }
}