//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using WCF = global::System.ServiceModel;

namespace Web09.Services.MessageContracts
{
	/// <summary>
	/// Service Contract Class - GetPupilAdjustmentReasonsResponse
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetPupilAdjustmentReasonsResponse", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetPupilAdjustmentReasonsResponse
	{
		private Web09.Services.DataContracts.AdjustmentReasons adjustmentReasons;
	 	private Web09.Services.DataContracts.Prompt prompt;
	 	private Web09.Services.DataContracts.CompletedPupilAdjustment completedAdjustment;
	 	private string priorMessage;
	 	private Web09.Services.DataContracts.CompletedPupilNonAdjustment completedNonAdjustment;
	 		
		[WCF::MessageBodyMember(Name = "AdjustmentReasons")] 
		public Web09.Services.DataContracts.AdjustmentReasons AdjustmentReasons
		{
			get { return adjustmentReasons; }
			set { adjustmentReasons = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Prompt")] 
		public Web09.Services.DataContracts.Prompt Prompt
		{
			get { return prompt; }
			set { prompt = value; }
		}
			
		[WCF::MessageBodyMember(Name = "CompletedAdjustment")] 
		public Web09.Services.DataContracts.CompletedPupilAdjustment CompletedAdjustment
		{
			get { return completedAdjustment; }
			set { completedAdjustment = value; }
		}
			
		[WCF::MessageBodyMember(Name = "PriorMessage")] 
		public string PriorMessage
		{
			get { return priorMessage; }
			set { priorMessage = value; }
		}
			
		[WCF::MessageBodyMember(Name = "CompletedNonAdjustment")] 
		public Web09.Services.DataContracts.CompletedPupilNonAdjustment CompletedNonAdjustment
		{
			get { return completedNonAdjustment; }
			set { completedNonAdjustment = value; }
		}
	}
}

