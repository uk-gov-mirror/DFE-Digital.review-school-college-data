namespace Dfe.Rscd.Api.Domain.Entities.ReferenceData
{
    public class PromptResponse
    {
        public int PromptID { get; set; }
        public int ListOrder { get; set; }
        public string ListValue { get; set; }
        public bool Rejected { get; set; }
    }
}