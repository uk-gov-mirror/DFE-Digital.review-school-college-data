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
	/// Service Contract Class - GetAllSchoolsResponse
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetAllSchoolsResponse
	{
		private Web09.Services.DataContracts.SmallSchoolsCollection smallSchoolList;
	 		
		[WCF::MessageBodyMember(Name = "SmallSchoolList")] 
		public Web09.Services.DataContracts.SmallSchoolsCollection SmallSchoolList
		{
			get { return smallSchoolList; }
			set { smallSchoolList = value; }
		}
	}
}

