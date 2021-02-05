using System;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class NotAllowedException : Exception
    {
        public string Title { get; set; }
        public string Detail { get; set; }

        public NotAllowedException(string title, string detail) : base(title)
        {
            Title = title;
            Detail = detail;
        }
    }
}
