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
	/// Service Contract Class - GetHelpListForPageRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetHelpListForPageRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/services")] 
	public partial class GetHelpListForPageRequest
	{
		private string pageName;
	 	private System.Nullable<bool> isJune;
	 	private System.Nullable<bool> isSept;
	 	private string userLevelID;
	 	private System.Collections.Generic.List<short> cohortIDList;
	 	private System.Collections.Generic.List<short> schoolGroupIDList;
	 		
		[WCF::MessageBodyMember(Name = "PageName")] 
		public string PageName
		{
			get { return pageName; }
			set { pageName = value; }
		}
			
		[WCF::MessageBodyMember(Name = "IsJune")] 
		public System.Nullable<bool> IsJune
		{
			get { return isJune; }
			set { isJune = value; }
		}
			
		[WCF::MessageBodyMember(Name = "IsSept")] 
		public System.Nullable<bool> IsSept
		{
			get { return isSept; }
			set { isSept = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserLevelID")] 
		public string UserLevelID
		{
			get { return userLevelID; }
			set { userLevelID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "CohortIDList")] 
		public System.Collections.Generic.List<short> CohortIDList
		{
			get { return cohortIDList; }
			set { cohortIDList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "SchoolGroupIDList")] 
		public System.Collections.Generic.List<short> SchoolGroupIDList
		{
			get { return schoolGroupIDList; }
			set { schoolGroupIDList = value; }
		}
	}
}

