namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentReason
    {
        public short ReasonId { get; set; }
        public string Description { get; set; }
        public short ParentReasonId { get; set; }
        
        public AmendmentType AmendmentType {get; set; }
    }

    public enum AmendmentReasonCode
    {
        AdmittedFromAbroadWithEnglishNotFirstLanguageCode = 8,
        AdmittedFollowingPermanentExclusion = 10,
        Deceased = 12,
        PermanentlyLeftEngland = 11,
        DualRegistration = 13,
        Other = 19,
        OtherEAL = 1901,
        OtherEHE = 1902,
        OtherPrisonRemandCentreSecureUnit = 1903,
        OtherPermanentlyExcludedThisSchool = 1904,
        OtherPoliceInvolvementBailRestrictions = 1905,
        OtherPupilMissingInEducation = 1906,
        OtherSafeguardingFAP = 1907,
        OtherTerminalIllness = 1908
    }
}
