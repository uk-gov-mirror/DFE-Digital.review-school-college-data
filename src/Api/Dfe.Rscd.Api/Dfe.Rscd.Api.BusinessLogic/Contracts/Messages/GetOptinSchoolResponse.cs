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
	/// Service Contract Class - GetOptinSchoolResponse
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetOptinSchoolResponse
	{
		private string jSON;
	 		
		[WCF::MessageBodyMember(Name = "JSON")] 
		public string JSON
		{
			get { return jSON; }
			set { jSON = value; }
		}
	}
}

