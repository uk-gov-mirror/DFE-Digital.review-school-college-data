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
	/// Service Contract Class - GetPupilRequestByPupilIDRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetPupilRequestByPupilIDRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetPupilRequestByPupilIDRequest
	{
		private int pupilID;
	 		
		[WCF::MessageBodyMember(Name = "PupilID")] 
		public int PupilID
		{
			get { return pupilID; }
			set { pupilID = value; }
		}
	}
}

