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
	/// Service Contract Class - GetResultAdjustmentRequestIDListResponse
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetResultAdjustmentRequestIDListResponse", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetResultAdjustmentRequestIDListResponse
	{
		private Web09.Services.DataContracts.ResultAdjustmentIDList resultAdjustmentRequestIDList;
	 	private int totalRowCount;
	 		
		[WCF::MessageBodyMember(Name = "ResultAdjustmentRequestIDList")] 
		public Web09.Services.DataContracts.ResultAdjustmentIDList ResultAdjustmentRequestIDList
		{
			get { return resultAdjustmentRequestIDList; }
			set { resultAdjustmentRequestIDList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "TotalRowCount")] 
		public int TotalRowCount
		{
			get { return totalRowCount; }
			set { totalRowCount = value; }
		}
	}
}

