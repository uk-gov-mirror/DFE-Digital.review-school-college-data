using System;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Common
{
    public class DateViewModel
    {
        public DateViewModel()
        {
        }

        public DateViewModel(DateTime date)
        {
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
        }

        public int? Day { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public DateTime? Date
        {
            get
            {
                try
                {
                    return new DateTime(Year.Value, Month.Value, Day.Value);
                }
                catch (Exception ex)
                when (ex is InvalidOperationException || ex is ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
        }
    }
}
