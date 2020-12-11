using System;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IAmendment
    {
        string URN { get; set; }
        AmendmentType AmendmentType { get; }
        string EvidenceFolderName { get; set; }
        EvidenceStatus EvidenceStatus { get; set; }
        IAmendmentDetail AmendmentDetail { get; set; }
        string Reference { get; set; }
        DateTime CreatedDate { get; set; }
        string Status { get; set; }
        string Id { get; set; }
        CheckingWindow CheckingWindow { get; set; }
        Pupil Pupil { get; set; }
    }
}