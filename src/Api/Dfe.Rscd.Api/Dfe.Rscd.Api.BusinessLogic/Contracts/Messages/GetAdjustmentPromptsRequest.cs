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
	/// Service Contract Class - GetAdjustmentPromptsRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetAdjustmentPromptsRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetAdjustmentPromptsRequest
	{
		private int pupilInclusionReasonID;
	 	private Web09.Services.DataContracts.PupilAdjustmentType pupilAdjustmentType;
	 	private int dFESNumber;
	 	private Web09.Services.DataContracts.PupilDetails pupilDetails;
	 	private Web09.Services.DataContracts.UserContext userContext;
	 		
		[WCF::MessageBodyMember(Name = "PupilInclusionReasonID")] 
		public int PupilInclusionReasonID
		{
			get { return pupilInclusionReasonID; }
			set { pupilInclusionReasonID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "PupilAdjustmentType")] 
		public Web09.Services.DataContracts.PupilAdjustmentType PupilAdjustmentType
		{
			get { return pupilAdjustmentType; }
			set { pupilAdjustmentType = value; }
		}
			
		[WCF::MessageBodyMember(Name = "DFESNumber")] 
		public int DFESNumber
		{
			get { return dFESNumber; }
			set { dFESNumber = value; }
		}
			
		[WCF::MessageBodyMember(Name = "PupilDetails")] 
		public Web09.Services.DataContracts.PupilDetails PupilDetails
		{
			get { return pupilDetails; }
			set { pupilDetails = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserContext")] 
		public Web09.Services.DataContracts.UserContext UserContext
		{
			get { return userContext; }
			set { userContext = value; }
		}
	}
}

