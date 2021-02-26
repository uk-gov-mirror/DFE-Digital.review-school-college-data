using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class PupilDTO
    {
        public string id { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
        public string DFESNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string DOB { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public int ENTRYDAT { get; set; }
        public string ActualYearGroup { get; set; }
        public bool Attendance_year_0 { get; set; }
        public bool Attendance_year_1 { get; set; }
        public bool Attendance_year_2 { get; set; }
        public string Core_Provider_0 { get; set; }
        public string Core_Provider_1 { get; set; }
        public string Core_Provider_2 { get; set; }
        public string SRC_LAESTAB_0 { get; set; }
        public string SRC_LAESTAB_1 { get; set; }
        public string SRC_LAESTAB_2 { get; set; }

        public string P_INCL { get; set; }
        public string EthnicityCode { get; set; }
        public string FirstLanguageCode { get; set; }
        public string SENStatusCode { get; set; }
        public int FSM6 { get; set; }
        public string ForvusIndex { get; set; }
        public int LookedAfterEver { get; set; }
        public int? PortlandStudentID { get; set; }
        
        public List<ResultDTO> performance { get; set; }

    }
}