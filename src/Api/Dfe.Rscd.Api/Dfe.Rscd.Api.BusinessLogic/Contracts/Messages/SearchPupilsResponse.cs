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
	/// Service Contract Class - SearchPupilsResponse
	/// </summary>
	[WCF::MessageContract(WrapperName = "SearchPupilsResponse", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class SearchPupilsResponse
	{
		private Web09.Services.DataContracts.PupilDetailsList pupilDetailsList;
	 		
		[WCF::MessageBodyMember(Name = "PupilDetailsList")] 
		public Web09.Services.DataContracts.PupilDetailsList PupilDetailsList
		{
			get { return pupilDetailsList; }
			set { pupilDetailsList = value; }
		}
	}
}

