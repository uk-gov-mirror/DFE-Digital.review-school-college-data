namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IAmendmentDetail
    {
        int ReasonCode { get; set; }

        string SubReason { get; set; }

        string Detail { get; set; }
    }
}