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
	/// Service Contract Class - GetPupilFilterListsRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetPupilFilterListsRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetPupilFilterListsRequest
	{
		private int dCSFNumber;
	 		
		[WCF::MessageBodyMember(Name = "DCSFNumber")] 
		public int DCSFNumber
		{
			get { return dCSFNumber; }
			set { dCSFNumber = value; }
		}
	}
}

