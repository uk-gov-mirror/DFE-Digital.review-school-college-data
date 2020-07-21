using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Entities;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb
{
    public class PupilDTO
    {
        public string id { get; set; }
        public string URN { get; set; }
        public string DFESNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        //public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        //public DateTime ENTRYDAT { get; set; }
        public string ActualYearGroup { get; set; }
        public string PostCode { get; set; }
        public string FSM6 { get; set; }
        //public List<Result> Results { get; set; }

        public Pupil Pupil => new Pupil
        {
            Id = new PupilId(id),
            Urn = new URN(URN),
            LaEstab = DFESNumber,
            ForeName = Forename,
            LastName = Surname,
            //DateOfBirth = DOB,
            Age = Age,
            Gender = Gender == "M" ? Domain.Core.Enums.Gender.Male : Domain.Core.Enums.Gender.Female,
            //DateOfAdmission = ENTRYDAT,
            YearGroup = ActualYearGroup,
            PostCode = PostCode,
            FSM6 = FSM6 == "1"
        };

    }
}
