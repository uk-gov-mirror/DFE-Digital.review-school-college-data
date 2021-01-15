namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
	public class Prompt 
	{
        public int PromptID { get; set; }

        public string PromptText { get; set; }

        public PromptType PromptType { get; set; }

        public PromptItemList PromptItemList { get; set; }

        public bool AllowNulls { get; set; }

        public string ColumnType { get; set; }

        public string PromptShortText { get; set; }

        public short UpdatedByDA { get; set; }
    }
}

