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
	/// Service Contract Class - GetSchoolGroupsResponse
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetSchoolGroupsResponse
	{
		private Web09.Services.DataContracts.SchoolGroups schoolGroups;
	 		
		[WCF::MessageBodyMember(Name = "SchoolGroups")] 
		public Web09.Services.DataContracts.SchoolGroups SchoolGroups
		{
			get { return schoolGroups; }
			set { schoolGroups = value; }
		}
	}
}

