using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;

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
        public int DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int ENTRYDAT { get; set; }
        public string ActualYearGroup { get; set; }

        public List<ResultDTO> performance { get; set; }

        public Pupil Pupil => new Pupil
        {
            Id = id,
            URN = URN,
            UPN = UPN,
            ULN = ULN,
            LaEstab = DFESNumber,
            ForeName = Forename,
            LastName = Surname,
            DateOfBirth = GetDateTime(DOB.ToString()),
            Age = Age,
            Gender = Gender == "M" ? Domain.Core.Enums.Gender.Male : Domain.Core.Enums.Gender.Female,
            DateOfAdmission = GetDateTime(ENTRYDAT.ToString()),
            YearGroup = ActualYearGroup,
            Results = performance.Select(p => new Result { SubjectCode = p.SubjectCode, ExamYear = p.ExamYear, TestMark = p.TestMark, ScaledScore = p.ScaledScore }).ToList()
        };

        private DateTime GetDateTime(string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyyMMdd", new CultureInfo("en-GB"), DateTimeStyles.None, out var date ) ? date : DateTime.MinValue;
        }
    }
}
