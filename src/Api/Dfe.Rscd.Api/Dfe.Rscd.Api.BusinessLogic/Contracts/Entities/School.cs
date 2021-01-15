using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
	public class School 
	{
        public int DfesNumber { get; set; }

        public URN Urn { get; set; }

		public string SchoolName { get; set; }

        public string SchoolType { get; set; }

        public KeyStageList KeyStageList { get; set; }

        public int? InstitutionTypeNumber { get; set; }

        public short SchoolStatus { get; set; }

        public bool IsSchoolClosed { get; set; }

        public string SchoolWebsite { get; set; }

        public string HeadTeacher { get; set; }

        public int? HighestAge { get; set; }

        public int? LowestAge { get; set; }

        public IList<PerformanceMeasure> CohortMeasures { get; set; }

        public IList<PerformanceMeasure> PerformanceMeasures { get; set; }
	}
}

