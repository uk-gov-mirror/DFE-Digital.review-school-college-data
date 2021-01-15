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
	/// Service Contract Class - IncludeResultRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "IncludeResultRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/services")] 
	public partial class IncludeResultRequest
	{
		private int studentID;
	 	private int qualificationID;
	 	private int year;
	 	private short keyStageID;
	 	private int pointID;
	 	private char seasonCode;
	 	private bool evidenceRequired;
	 	private System.Nullable<int> marks;
	 	private Web09.Services.DataContracts.UserContext userContext;
	 	private bool acceptOnEntry;
	 	private string gradeText;
	 		
		[WCF::MessageBodyMember(Name = "StudentID")] 
		public int StudentID
		{
			get { return studentID; }
			set { studentID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "QualificationID")] 
		public int QualificationID
		{
			get { return qualificationID; }
			set { qualificationID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Year")] 
		public int Year
		{
			get { return year; }
			set { year = value; }
		}
			
		[WCF::MessageBodyMember(Name = "KeyStageID")] 
		public short KeyStageID
		{
			get { return keyStageID; }
			set { keyStageID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "PointID")] 
		public int PointID
		{
			get { return pointID; }
			set { pointID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "SeasonCode")] 
		public char SeasonCode
		{
			get { return seasonCode; }
			set { seasonCode = value; }
		}
			
		[WCF::MessageBodyMember(Name = "EvidenceRequired")] 
		public bool EvidenceRequired
		{
			get { return evidenceRequired; }
			set { evidenceRequired = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Marks")] 
		public System.Nullable<int> Marks
		{
			get { return marks; }
			set { marks = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserContext")] 
		public Web09.Services.DataContracts.UserContext UserContext
		{
			get { return userContext; }
			set { userContext = value; }
		}
			
		[WCF::MessageBodyMember(Name = "AcceptOnEntry")] 
		public bool AcceptOnEntry
		{
			get { return acceptOnEntry; }
			set { acceptOnEntry = value; }
		}
			
		[WCF::MessageBodyMember(Name = "GradeText")] 
		public string GradeText
		{
			get { return gradeText; }
			set { gradeText = value; }
		}
	}
}

