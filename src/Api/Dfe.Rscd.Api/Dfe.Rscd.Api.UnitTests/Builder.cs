using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using DTO = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;

namespace Dfe.Rscd.Api.UnitTests
{
    public class Builder
    {
        public static PupilDTO GetPupilDTO(string id, string gender, string urn, string upn, string uln, string name="Jack", string surname="Smith")
        {
            return new PupilDTO
            {
                id = id,
                Gender = gender,
                ActualYearGroup = "2020",
                Age = 14,
                Attendance_year_0 = true,
                Attendance_year_1 = false,
                Attendance_year_2 = false,
                Core_Provider_0 = "123232",
                Core_Provider_1 = "444433",
                Core_Provider_2 = "443344",
                DFESNumber = "123232",
                DOB = "20060413",
                ENTRYDAT = 20200101,
                Forename = name,
                SRC_LAESTAB_0 = "AO",
                SRC_LAESTAB_1 = "AO",
                SRC_LAESTAB_2 = "AO",
                Surname = surname,
                ULN = uln,
                UPN = upn,
                URN = urn,
                performance = new List<ResultDTO>
                {
                    new ResultDTO {ExamYear = "2020", ScaledScore = "A", SubjectCode = "YUG", TestMark = "B"}
                },
                ForvusIndex = "10020",
                AdoptedFromCareID = "1",
                EthnicityCode = "WBR01",
                FSM = "1",
                FirstLanguageCode = "ENG",
                P_INCL = "404",
                SENStatusCode = "N"
            };
        }

        public static List<PupilDTO> GetPupils()
        {
            return new List<PupilDTO>
            {
                GetPupilDTO("1000", "M", "U111", "111", "10100", "John", "Patrick"),
                GetPupilDTO("2000", "F", "U222", "222", "20200", "Cecil", "Warrington"),
                GetPupilDTO("3000", "M", "U333", "333", "30300", "Marcus", "Russel"),
                GetPupilDTO("4000", "F", "U444", "444", "40400", "Mary", "Blight")
            };
        }

        public static List<EstablishmentDTO> GetEstablishment(int dfesNumber)
        {
            return new List<EstablishmentDTO>
            {
                new EstablishmentDTO
                {
                    DFESNumber = dfesNumber.ToString(),
                    SchoolName = "St Johns High School",
                    HeadFirstName = "Mr",
                    HeadLastName = "John",
                    HeadTitleCode = "Williams",
                    id = "123456",
                    performance = new List<EstablishmentDTO.PerformanceDto>()
                }
            };
        }

        public static EstablishmentDTO GetEstablishment(string urn)
        {
            return new EstablishmentDTO
            {
                DFESNumber = "99"+urn,
                HeadFirstName = "Marcus",
                HeadLastName = "Russel",
                HeadTitleCode = "Mr",
                HighestAge = 18,
                InstitutionTypeNumber = 11,
                LowestAge = 12,
                SchoolName = "St Marys School",
                SchoolStatusCode = 1,
                SchoolType = "Maintained School",
                WebsiteAddress = "www.address.com",
                id = urn,
                performance = new List<EstablishmentDTO.PerformanceDto>
                {
                    new EstablishmentDTO.PerformanceDto
                    {
                        Code = "N01",
                        CodeValue = "V01"
                    }
                }
                
            };
        }

        internal static List<DTO.Pincl> GetPincLsList()
        {
            return new List<DTO.Pincl>
            {
                new DTO.Pincl{ PIncl1 = "404", PIncldescription = "Included at end of Keystage 4", DisplayFlag = "1"}
            };
        }

        internal static List<DTO.Senstatus> GetSenList()
        {
            return new List<DTO.Senstatus>
            {
                new DTO.Senstatus { SenstatusCode = "N", SenstatusDescription = "None"}
            };
        }

        internal static List<DTO.Language> GetLanguageList()
        {
            return new List<DTO.Language>
            {
                new DTO.Language{ LanguageCode = "ENG", LanguageDescription = "English"}
            };
        }

        internal static List<DTO.Ethnicity> GetEthnicityList()
        {
            return new List<DTO.Ethnicity>
            {
                new DTO.Ethnicity{ EthnicityCode = "WBR01", EthnicityDescription = "White British"}
            };
        }

        public static List<EstablishmentDTO> GetEstablisments()
        {
            return new List<EstablishmentDTO>
            {
                GetEstablishment("100000"),
                GetEstablishment("200000"),
                GetEstablishment("300000")
            };
        }
    }
}