using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Domain.Core
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
