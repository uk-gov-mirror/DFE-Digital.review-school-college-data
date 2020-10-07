using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Amendment
    {
        public CheckingWindow CheckingWindow { get; set; }
        public AmendmentType AmendmentType { get; set; }
        public string URN { get; set; }
        public Pupil Pupil { get; set; }
        public Evidence Evidence { get; set; }
    }
}
