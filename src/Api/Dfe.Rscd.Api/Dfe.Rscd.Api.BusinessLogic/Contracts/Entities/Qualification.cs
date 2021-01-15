namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
	public class Qualification 
	{
		private int qualificationID;
		private string qualificationCode;
		private string qualificationName;
		private int aPR15;
		private int aPR1618;
		private bool kS4Main;
		private bool kS5Main;
		private int yearStart;
		private int yearDeleted;
		private int subLevelId;
		
		public int QualificationID
		{
		  get { return qualificationID; }
		  set { qualificationID = value; }
		}				
		
		public string QualificationCode
		{
		  get { return qualificationCode; }
		  set { qualificationCode = value; }
		}				
		
		public string QualificationName
		{
		  get { return qualificationName; }
		  set { qualificationName = value; }
		}				
		
		public int APR15
		{
		  get { return aPR15; }
		  set { aPR15 = value; }
		}				
		
		public int APR1618
		{
		  get { return aPR1618; }
		  set { aPR1618 = value; }
		}				
		
		public bool KS4Main
		{
		  get { return kS4Main; }
		  set { kS4Main = value; }
		}				
		
		public bool KS5Main
		{
		  get { return kS5Main; }
		  set { kS5Main = value; }
		}				
		
		public int YearStart
		{
		  get { return yearStart; }
		  set { yearStart = value; }
		}				
		
		public int YearDeleted
		{
		  get { return yearDeleted; }
		  set { yearDeleted = value; }
		}				
		
		public int SubLevelId
		{
		  get { return subLevelId; }
		  set { subLevelId = value; }
		}				
	}
}

