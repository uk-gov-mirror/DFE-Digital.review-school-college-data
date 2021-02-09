using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;

namespace Dfe.Rscd.Api.Services
{
    public class PupilService : IPupilService
    {
        // TODO: Decide a max pagesize for now, can't return all pupils
        private const int PageSize = 200;
        private readonly string _allocationYear;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDataService _dataService;
        private readonly IEstablishmentService _schoolService;

        public PupilService(IDocumentRepository documentRepository, IDataService dataService, IEstablishmentService schoolService, IAllocationYearConfig year)
        {
            _documentRepository = documentRepository;
            _dataService = dataService;
            _schoolService = schoolService;
            _allocationYear = year.Value;
        }

        public Pupil GetById(CheckingWindow checkingWindow, string id)
        {
            var pupilDTO = _documentRepository.GetById<PupilDTO>(GetCollection(checkingWindow), id);
            return GetPupil(checkingWindow, pupilDTO, _allocationYear);
        }

        public List<PupilRecord> QueryPupils(CheckingWindow checkingWindow, PupilsSearchRequest query)
        {
            var repoQuery = _documentRepository.Get<PupilDTO>(GetCollection(checkingWindow));
            
            if (!string.IsNullOrWhiteSpace(query.URN))
                repoQuery = repoQuery.Where(p => p.URN == query.URN);
            if (!string.IsNullOrWhiteSpace(query.ID))
                repoQuery = repoQuery.Where(p => p.UPN.StartsWith(query.ID) || p.ULN.StartsWith(query.ID));
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var nameParts = query.Name.Split(' ');
                foreach (var namePart in nameParts)
                    repoQuery = repoQuery.Where(p => p.Forename.StartsWith(namePart, StringComparison.InvariantCultureIgnoreCase) || 
                                                     p.Surname.StartsWith(namePart, StringComparison.InvariantCultureIgnoreCase));
            }

            var dtos = repoQuery
                .Select(p => new PupilRecord
                {
                    Id = p.id, 
                    ForeName = p.Forename, 
                    Surname = p.Surname, 
                    ULN = p.ULN, 
                    UPN = p.UPN, 
                    Gender=p.Gender, 
                    DateOfBirth = p.DOB
                }).Take(PageSize);

            return dtos.ToList();
        }

        private string GetCollection(CheckingWindow checkingWindow)
        {
            return $"{checkingWindow.ToString().ToLower()}_pupils_{_allocationYear}";
        }

        public Pupil GetPupil(CheckingWindow checkingWindow, PupilDTO pupil, string allocationYear)
        {
            var sen = _dataService.GetSENStatus().SingleOrDefault(x=>x.Code == pupil.SENStatusCode);
            var pincl = _dataService.GetPINCLs().SingleOrDefault(x => x.Code == pupil.P_INCL);
            var ethnicity = _dataService.GetEthnicities().SingleOrDefault(x => x.Code == pupil.EthnicityCode);
            var language = _dataService.GetLanguages().SingleOrDefault(x => x.Code == pupil.FirstLanguageCode);
            var school = _schoolService.GetByDFESNumber(checkingWindow, pupil.DFESNumber);

            var newPupil = new Pupil
            {
                Id = pupil.id,
                URN = pupil.URN,
                UPN = pupil.UPN,
                ULN = pupil.ULN,
                DfesNumber = pupil.DFESNumber,
                Forename = pupil.Forename,
                Surname = pupil.Surname,
                DOB = GetDateTime(pupil.DOB),
                Age = pupil.Age ?? 0,
                Gender = Gender.FromCode(pupil.Gender),
                AdmissionDate = GetDateTime(pupil.ENTRYDAT.ToString()),
                YearGroup = pupil.ActualYearGroup,
                Results = pupil.performance.Select(p => new Result
                {
                    SubjectCode = p.SubjectCode,
                    ExamYear = p.ExamYear,
                    TestMark = p.TestMark,
                    ScaledScore = p.ScaledScore
                }).ToList(),
                Allocations = GetSourceOfAllocations(pupil, allocationYear),
                ForvusIndex = int.Parse(pupil.ForvusIndex),
                LookedAfterEver = pupil.LookedAfterEver == 1,
                School = school,
                FreeSchoolMealsLast6Years = pupil.FSM6 == 1
            };

            if (sen != null) newPupil.SpecialEducationNeed = sen;
            if (ethnicity != null) newPupil.Ethnicity = ethnicity;
            if (language != null) newPupil.FirstLanguage = language;
            if (ethnicity != null) newPupil.Ethnicity = ethnicity;
            if (pincl != null) newPupil.PINCL = pincl;

            return newPupil;
        }

        private int ConvertId(string id)
        {
            if (int.TryParse(id, out var idParsed))
            {
                return idParsed;
            }

            return 0;
        }

        public PupilRecord GetPupilRecord(PupilDTO pupil)
        {
            return new PupilRecord
            {
                Id = pupil.id,
                ForeName = pupil.Forename,
                Surname = pupil.Surname,
                ULN = pupil.ULN,
                UPN = pupil.UPN
            };
        }

        private List<SourceOfAllocation> GetSourceOfAllocations(PupilDTO pupil, string allocationYear)
        {
            var year = int.Parse(allocationYear);
            var allocations = new List<SourceOfAllocation>();
            if (string.IsNullOrEmpty(pupil.SRC_LAESTAB_0) && string.IsNullOrEmpty(pupil.SRC_LAESTAB_1) &&
                string.IsNullOrEmpty(pupil.SRC_LAESTAB_2)) return allocations;

            if (pupil.Attendance_year_0 && pupil.DFESNumber == pupil.Core_Provider_0)
                allocations.Add(new SourceOfAllocation(year--, pupil.SRC_LAESTAB_0.ToAllocation()));
            else
                allocations.Add(new SourceOfAllocation(year--,
                    string.IsNullOrEmpty(pupil.Core_Provider_0) ? Allocation.Unknown : Allocation.NotAllocated));

            if (pupil.Attendance_year_1 && pupil.DFESNumber == pupil.Core_Provider_1)
                allocations.Add(new SourceOfAllocation(year--, pupil.SRC_LAESTAB_1.ToAllocation()));
            else
                allocations.Add(new SourceOfAllocation(year--,
                    string.IsNullOrEmpty(pupil.Core_Provider_1) ? Allocation.Unknown : Allocation.NotAllocated));

            if (pupil.Attendance_year_2 && pupil.DFESNumber == pupil.Core_Provider_2)
                allocations.Add(new SourceOfAllocation(year--, pupil.SRC_LAESTAB_2.ToAllocation()));
            else
                allocations.Add(new SourceOfAllocation(year--,
                    string.IsNullOrEmpty(pupil.Core_Provider_2) ? Allocation.Unknown : Allocation.NotAllocated));

            return allocations;
        }

        private DateTime GetDateTime(string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyyMMdd", new CultureInfo("en-GB"), DateTimeStyles.None,
                out var date)
                ? date
                : DateTime.MinValue;
        }
    }
}