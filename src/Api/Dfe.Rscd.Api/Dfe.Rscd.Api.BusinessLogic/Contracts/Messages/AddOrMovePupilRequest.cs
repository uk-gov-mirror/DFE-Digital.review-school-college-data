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
	/// Service Contract Class - AddOrMovePupilRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "AddOrMovePupilRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class AddOrMovePupilRequest
	{
		private Web09.Services.DataContracts.PupilDetails pupilDetails;
	 	private Web09.Services.DataContracts.CompletedPupilAdjustment completedPupilAdjustment;
	 	private Web09.Services.DataContracts.UserContext userContext;
	 		
		[WCF::MessageBodyMember(Name = "PupilDetails")] 
		public Web09.Services.DataContracts.PupilDetails PupilDetails
		{
			get { return pupilDetails; }
			set { pupilDetails = value; }
		}
			
		[WCF::MessageBodyMember(Name = "CompletedPupilAdjustment")] 
		public Web09.Services.DataContracts.CompletedPupilAdjustment CompletedPupilAdjustment
		{
			get { return completedPupilAdjustment; }
			set { completedPupilAdjustment = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserContext")] 
		public Web09.Services.DataContracts.UserContext UserContext
		{
			get { return userContext; }
			set { userContext = value; }
		}
	}
}

