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
	/// Service Contract Class - GetSchoolValueRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetSchoolValueRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetSchoolValueRequest
	{
		private int dFESNumber;
	 	private string schoolValueCode;
	 	private int keyStage;
	 		
		[WCF::MessageBodyMember(Name = "DFESNumber")] 
		public int DFESNumber
		{
			get { return dFESNumber; }
			set { dFESNumber = value; }
		}
			
		[WCF::MessageBodyMember(Name = "SchoolValueCode")] 
		public string SchoolValueCode
		{
			get { return schoolValueCode; }
			set { schoolValueCode = value; }
		}
			
		[WCF::MessageBodyMember(Name = "KeyStage")] 
		public int KeyStage
		{
			get { return keyStage; }
			set { keyStage = value; }
		}
	}
}

