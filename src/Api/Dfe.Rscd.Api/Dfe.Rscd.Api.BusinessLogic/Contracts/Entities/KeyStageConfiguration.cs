using WcfSerialization = global::System.Runtime.Serialization;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
	public class KeyStageConfiguration 
	{
        public short KeyStage { get; set; }

        public string ConfigurationCode { get; set; }

        public string ConfigurationDescription { get; set; }

        public string ConfigurationValue { get; set; }
    }
}

