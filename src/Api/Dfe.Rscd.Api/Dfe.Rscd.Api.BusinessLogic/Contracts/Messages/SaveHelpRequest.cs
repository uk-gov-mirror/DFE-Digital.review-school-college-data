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
	/// Service Contract Class - SaveHelpRequest
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class SaveHelpRequest
	{
		private Web09.Services.DataContracts.Help help;
	 		
		[WCF::MessageBodyMember(Name = "Help")] 
		public Web09.Services.DataContracts.Help Help
		{
			get { return help; }
			set { help = value; }
		}
	}
}

