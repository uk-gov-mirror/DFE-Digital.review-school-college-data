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
	/// Service Contract Class - GetResultsForSchoolResponse
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetResultsForSchoolResponse
	{
		private Web09.Services.DataContracts.Results results;
	 		
		[WCF::MessageBodyMember(Name = "Results")] 
		public Web09.Services.DataContracts.Results Results
		{
			get { return results; }
			set { results = value; }
		}
	}
}

