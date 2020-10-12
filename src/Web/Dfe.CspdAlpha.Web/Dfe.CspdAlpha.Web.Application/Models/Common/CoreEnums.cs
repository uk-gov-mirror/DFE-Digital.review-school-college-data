using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.CspdAlpha.Web.Application.Models.Common
{
    public enum Gender
    {
        Unknown,
        Female,
        Male
    }
    public enum AddReason
    {
        Unknown,
        New,
        Existing
    }

    public enum CheckingWindow
    {
        Unknown,
        KS2,
        KS2Errata,
        KS4June,
        KS4Late,
        KS4Errata,
        KS5,
        KS5Errata
    }

    public enum Keystage
    {
        Unknown,
        KS2,
        KS4,
        KS5
    }
}
