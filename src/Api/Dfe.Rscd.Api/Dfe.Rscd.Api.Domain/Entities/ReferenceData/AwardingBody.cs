namespace Dfe.Rscd.Api.Domain.Entities.ReferenceData
{
    public class AwardingBody
    {
        public int AwardingBodyID { get; set; }
        public string AwardingBodyNumber { get; set; }
        public string AwardingBodyCode { get; set; }
        public string AwardingBodyName { get; set; }
        public string Was_Other { get; set; }
        public bool DoesGradedExams { get; set; }
    }
}