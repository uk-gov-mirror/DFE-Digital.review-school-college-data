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
	/// Service Contract Class - GetPilotSchoolDocumentsResponse
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetPilotSchoolDocumentsResponse
	{
		private Web09.Services.DataContracts.SchoolDocuments documentsList;
	 		
		[WCF::MessageBodyMember(Name = "DocumentsList")] 
		public Web09.Services.DataContracts.SchoolDocuments DocumentsList
		{
			get { return documentsList; }
			set { documentsList = value; }
		}
	}
}

