using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments
{
    public interface IAmendmentType
    {
        AmendmentType AmendmentType { get; }
        PupilDetails PupilDetails { get; set; }
    }
}
