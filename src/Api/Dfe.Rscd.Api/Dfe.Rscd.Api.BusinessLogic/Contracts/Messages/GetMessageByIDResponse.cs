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
	/// Service Contract Class - GetMessageByIDResponse
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetMessageByIDResponse
	{
		private Web09.Services.DataContracts.Message message;
	 		
		[WCF::MessageBodyMember(Name = "Message")] 
		public Web09.Services.DataContracts.Message Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}

