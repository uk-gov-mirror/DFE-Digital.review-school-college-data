﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// Created via this command line: "C:\Users\arose\AppData\Roaming\MscrmTools\XrmToolBox\Plugins\DLaB.EarlyBoundGenerator\crmsvcutil.exe" /url:"https://rscd-dev.api.crm4.dynamics.com/XRMServices/2011/Organization.svc" /namespace:"Dfe.CspdAlpha.Web.Infrastructure.Crm" /out:"C:\Users\arose\OneDrive - Department for Education\Downloads\EBG\OptionSets.cs" /codecustomization:"DLaB.CrmSvcUtilExtensions.OptionSet.CustomizeCodeDomService,DLaB.CrmSvcUtilExtensions" /codegenerationservice:"DLaB.CrmSvcUtilExtensions.OptionSet.CustomCodeGenerationService,DLaB.CrmSvcUtilExtensions" /codewriterfilter:"DLaB.CrmSvcUtilExtensions.OptionSet.CodeWriterFilterService,DLaB.CrmSvcUtilExtensions" /namingservice:"DLaB.CrmSvcUtilExtensions.NamingService,DLaB.CrmSvcUtilExtensions" /metadataproviderservice:"DLaB.CrmSvcUtilExtensions.BaseMetadataProviderService,DLaB.CrmSvcUtilExtensions" /username:"andy.rose@education.gov.uk" /password:"***************" 
//------------------------------------------------------------------------------

namespace Dfe.CspdAlpha.Web.Infrastructure.Crm
{


	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_Addreason
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Existing pupil", 1, "#0000ff")]
		Existingpupil = 353910001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("New pupil", 0, "#0000ff")]
		Newpupil = 353910000,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_Decision
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Approved", 0)]
		Approved = 978790000,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Escalated and approved", 2)]
		Escalatedandapproved = 100000001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Escalated and rejected", 3)]
		Escalatedandrejected = 353910001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Rejected", 1)]
		Rejected = 978790001,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_establishment_StatusCode
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Active", 0)]
		Active = 1,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Inactive", 1)]
		Inactive = 2,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_EvidenceOption
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("DontUploadEvidence", 2)]
		DontUploadEvidence = 978790002,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("UploadEvidenceLater", 1)]
		UploadEvidenceLater = 978790001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("UploadEvidenceNow", 0)]
		UploadEvidenceNow = 978790000,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_Finaldecision
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Approved", 0)]
		Approved = 353910000,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Rejected", 1)]
		Rejected = 353910001,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_Gender
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Female", 1)]
		Female = 978790001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Male", 0)]
		Male = 978790000,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum cr3d5_Pupiltype
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Existing pupil", 1, "#0000ff")]
		Existingpupil = 353910001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("New pupil", 0, "#0000ff")]
		Newpupil = 353910000,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum new_Amendment_rscd_Amendmenttype
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Add pupil", 0, "#0000ff")]
		Addpupil = 501940000,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Remove pupil", 1, "#0000ff")]
		Removepupil = 501940001,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum new_Amendment_StatusCode
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Active", 0)]
		Active = 1,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Inactive", 1)]
		Inactive = 2,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum new_amendmentStatus
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Accepted", 1)]
		Accepted = 100000001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("AwaitingEvidence", 4)]
		AwaitingEvidence = 100000004,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Cancelled", 3)]
		Cancelled = 100000003,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Rejected", 2)]
		Rejected = 100000002,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Requested", 0)]
		Requested = 100000000,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum new_reviewandconfirmschool_StatusCode
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Active", 0)]
		Active = 1,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Inactive", 1)]
		Inactive = 2,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Addpupil_StatusCode
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Active", 0)]
		Active = 1,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Inactive", 1)]
		Inactive = 2,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Amendment_StatusCode
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Active", 0)]
		Active = 1,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Inactive", 1)]
		Inactive = 2,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Amendmenttype
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Add a pupil", 0)]
		Addapupil = 501940000,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Add or amend a result", 2)]
		Addoramendaresult = 501940002,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Remove a pupil", 1)]
		Removeapupil = 501940001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Remove a result", 3)]
		Removearesult = 501940003,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Checkingwindow
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS2", 0)]
		KS2 = 501940000,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS2Errata", 1)]
		KS2Errata = 501940001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS4Errata", 4)]
		KS4Errata = 501940004,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS4June", 2)]
		KS4June = 501940002,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS4Late", 3)]
		KS4Late = 501940003,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS5", 5)]
		KS5 = 501940005,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("KS5Errata", 6)]
		KS5Errata = 501940006,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Evidencestatus
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Later", 1)]
		Later = 501940001,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Not required", 2)]
		Notrequired = 501940002,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Now", 0)]
		Now = 501940000,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Outcome
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Approved", 0)]
		Approved = 501940000,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Awaiting DfE review", 2)]
		AwaitingDfEreview = 501940002,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Rejected", 1)]
		Rejected = 501940001,
	}

	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum rscd_Removepupil_StatusCode
	{

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Active", 0)]
		Active = 1,

		[System.Runtime.Serialization.EnumMemberAttribute()]
		[OptionSetMetadataAttribute("Inactive", 1)]
		Inactive = 2,
	}
}
