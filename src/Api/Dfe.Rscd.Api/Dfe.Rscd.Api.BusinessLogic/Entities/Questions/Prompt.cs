using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
	public class Prompt 
	{
        public int PromptID { get; set; }

        public string PromptText { get; set; }

        public string PromptType { get; set; }

        public List<PromptItem> PromptItemList { get; set; }

        public bool AllowNulls { get; set; }

        public string ColumnType { get; set; }

        public string PromptShortText { get; set; }
    }
}

