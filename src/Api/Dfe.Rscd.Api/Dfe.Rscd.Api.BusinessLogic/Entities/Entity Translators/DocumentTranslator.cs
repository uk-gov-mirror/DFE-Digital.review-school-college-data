using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using Web09.Checking.Infrastructure.CosmosDb.DTOs;
using Web09.Services.Common.JSONObjects;
using Web09.Services.DataContracts;
using Pupil = Web09.Services.Common.JSONObjects.Pupil;

namespace Web09.Services.StubAdapters.Entity_Translators
{
    public class DocumentTranslator
    {
        public static School TranslateSchoolFromDTO(EstablishmentDTO school)
        {
            return new School
            {
                DfesNumber = Convert<int>(school.DFESNumber),
                SchoolName = school.SchoolName,
                HeadTeacher = $"{school.HeadTitleCode} {school.HeadFirstName} {school.HeadLastName}",
                HighestAge = Convert<short>(school.HighestAge),
                InstitutionTypeNumber = school.InstitutionTypeNumber,
                IsSchoolClosed = school.SchoolStatusCode != 0,
                KeyStageList = new KeyStageList
                    {new KeyStageDetails {KeyStage = 4, KeyStageName = "KS4", KeystagePupilNomenclature = "KS4"}},
                LowestAge = Convert<short>(school.LowestAge),
                SchoolStatus = Convert<short>(school.SchoolStatusCode),
                SchoolWebsite = school.WebsiteAddress,
                SchoolGroups = new SchoolGroups { new SchoolGroup { SchoolGroupName = school.SchoolType, SchoolGroupDescription = school.SchoolType} }
            };
        }

        public static Schools TranslateFromSchoolDTOList(IList<EstablishmentDTO> schoolsDtos)
        {
            var schools = new Schools();
            schools.AddRange(schoolsDtos.Select(TranslateSchoolFromDTO));
            return schools;
        }

        public static SmallSchoolsCollection TranlateSmallSchoolsFromDTOs(IList<EstablishmentProxyDTO> schools)
        {
            var smallSchoolsCollection = new SmallSchoolsCollection();
            smallSchoolsCollection.AddRange(schools.Select(x => new SmallSchool
                {DFESNumber = Convert<int>(x.DFESNumber), SchoolName = x.SchoolName}));
            return smallSchoolsCollection;
        }

        public static T Convert<T>(object input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (!string.IsNullOrEmpty("" + input))
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T) converter.ConvertFromString(input.ToString());
                return default;
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }

        public static string GetJson<T>(T values)
        {
            var jsSerializer = new JavaScriptSerializer();
            var json = jsSerializer.Serialize(values);
            return json;
        }

        private static DateTime GetDateTime(string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyyMMdd", new CultureInfo("en-GB"), DateTimeStyles.None,
                out var date)
                ? date
                : DateTime.MinValue;
        }

        public static PupilAttainmentItemCollection TranslateAttainmentFromDTO(List<PerformanceDto> schoolPerformance)
        {
            var pupilAttainmentCollection = new PupilAttainmentItemCollection();
            var index = 0;
            foreach (var perf in schoolPerformance)
            {
                pupilAttainmentCollection.Add(new PupilAttainmentItem
                {
                    AttainmentIndicator = perf.Code,
                    AttainmentIndicatorCode = perf.Code,
                    ListOrder = index,
                    RawValue = perf.CodeValue,
                    StandardizedValue = perf.CodeValue,
                    Weight = 0
                });

                index++;
            }

            return pupilAttainmentCollection;
        }

        public static SchoolAddress TranslateAddressFromDTO(AddressDTO schoolAdd)
        {
            return new SchoolAddress
            {
                DFESNumber = Convert<int>(schoolAdd.DFESNumber),
                Line1 = schoolAdd.ADDRESS1,
                Line2 = schoolAdd.ADDRESS2,
                Line3 = schoolAdd.ADDRESS3,
                Town = schoolAdd.TOWN,
                County = schoolAdd.COUNTY,
                PostCode = schoolAdd.PCODE,
                PhoneNumber = schoolAdd.TELNUM
            };
        }

        public static PupilSearchResult TranslatePupilSearchItemResultFromDTO(PupilDTO pupil, string schoolName,
            short keyStage)
        {
            return new PupilSearchResult
            {
                AdmissionDate = GetDateTime(pupil.ENTRYDAT.ToString()).ToShortDateString(),
                Age = Convert<byte>(pupil.Age ?? 0),
                DFESNumber = Convert<int>(pupil.DFESNumber),
                SchoolName = schoolName,
                EthnicityCode = pupil.EthnicityCode,
                FirstLanguageCode = pupil.FirstLanguageCode,
                Surname = pupil.Surname,
                PINCLDescription = pupil.P_INCL,
                DOB = GetDateTime(pupil.DOB).ToShortDateString(),
                Forename = pupil.Forename,
                Gender = pupil.Gender == "M" ? "M" : "F",
                KeyStage = keyStage,
                PINCLCode = pupil.P_INCL,
                PINCLDisplayFlag = "√",
                StudentID = Convert<int>(pupil.CandidateNumber),
                YearGroup = Convert<int>(pupil.ActualYearGroup)
            };
        }

        public static string TranslatePupilSearchJSONResultFromDTO(IList<PupilDTO> pupils, string schoolName,
            short keyStage)
        {
            var pupilSearchResultList = new List<PupilSearchResult>();
            pupilSearchResultList.AddRange(pupils.Select(x =>
                TranslatePupilSearchItemResultFromDTO(x, schoolName, keyStage)));

            return GetJson(pupilSearchResultList);
        }

        public static PupilDetails TranslatePupilFromDTO(PupilDTO pupil, string schoolName, short keyStage)
        {
            return new PupilDetails
            {
                Forename = pupil.Forename,
                Surname = pupil.Surname,
                AdmissionDate = GetDateTime(pupil.ENTRYDAT.ToString()),
                AdmissionDateDisplayString = GetDateTime(pupil.ENTRYDAT.ToString()).ToShortDateString(),
                Age = Convert<byte>(pupil.Age ?? 0),
                DOB = GetDateTime(pupil.DOB),
                DOBDisplayString = GetDateTime(pupil.DOB).ToShortDateString(),
                Gender = pupil.Gender == "M" ? 'M' : 'F',
                InCare = pupil.AdoptedFromCareID != "0",
                YearGroup = pupil.ActualYearGroup,
                SchoolName = schoolName,
                SchoolDFESNumber = Convert<int>(pupil.DFESNumber),
                EthnicityCode = pupil.EthnicityCode,
                FirstLanguageCode = pupil.FirstLanguageCode,
                SENStatusCode = pupil.SENStatusCode,
                KeyStage = keyStage,
                ForvusNumber = Convert<int>(pupil.ForvusIndex),
                FreeSchoolMeals = pupil.FSM == "1",
                PINCLCode = pupil.P_INCL,
                PINCLDescription = pupil.P_INCL,
                PINCLDisplayFlag = "√",
                PostCode = pupil.PostCode,
                PupilID = Convert<int>(pupil.CandidateNumber),
                UPN = pupil.UPN
            };
        }

        public static CohortDetails TranslateCohortDetailsFromDTO(PupilDTO pupil)
        {
            return new CohortDetails
            {
                Forename = pupil.Forename,
                Surname = pupil.Surname,
                StudentID = Convert<int>(pupil.CandidateNumber),
                Age = Convert<byte>(pupil.Age ?? 0),
                Gender = pupil.Gender == "M" ? "M" : "F",
                PINCLDescription = pupil.P_INCL,
                PINCLDisplayFlag = "√",
                DOBDisplayString = GetDateTime(pupil.DOB).ToShortDateString(),
                AdmissionDateDisplayString = GetDateTime(pupil.ENTRYDAT.ToString()).ToShortDateString(),
                ForvusNumber = Convert<int>(pupil.ForvusIndex),
                HasOldValues = false,
                HasResultAmendments = false,
                EthnicityCode = pupil.EthnicityCode,
                FirstLanguageCode = pupil.FirstLanguageCode,
                FreeSchoolMeals = pupil.FSM,
                InCare = pupil.AdoptedFromCareID,
                SENStatusCode = pupil.SENStatusCode,
                YearGroup = pupil.ActualYearGroup,
            };
        }

        public static string TranslateCohortDetailsV2FromDTO(List<PupilDTO> pupilsDTO)
        {
            var pupils = pupilsDTO.Select(TranslateCohortDetailsFromDTO);
            return GetJson(pupils);
        }

        public static Pupil TranslatePupilV2FromDTO(PupilDTO pupil, string schoolName, short keyStage)
        {
            return new Pupil
            {
                SchoolName = schoolName,
                EthnicityCode = pupil.EthnicityCode,
                FirstLanguageCode = pupil.FirstLanguageCode,
                SENStatusCode = pupil.SENStatusCode,
                KeyStage = keyStage,
                Surname = pupil.Surname,
                AdoptedFromCareCode = pupil.AdoptedFromCareID,
                AdoptedFromCareDescription = pupil.AdoptedFromCareID,
                DateOfBirth = GetDateTime(pupil.DOB),
                Forename = pupil.Forename,
                ForvusNumber = Convert<int>(pupil.ForvusIndex),
                FreeSchoolMeals = pupil.FSM == "1",
                Gender = pupil.Gender == "M" ? "M" : "F",
                InCare = pupil.AdoptedFromCareID != "0",
                PINCDescription = pupil.P_INCL,
                PINCLCode = pupil.P_INCL,
                PINCLDisplayFlag = pupil.P_INCL,
                PostCode = pupil.PostCode,
                PupilID = Convert<int>(pupil.CandidateNumber),
                YearGroup = Convert<int>(pupil.ActualYearGroup),
                UPN = pupil.UPN,
                SchoolDfesNumber = Convert<int>(pupil.DFESNumber)
            };
        }

        public static PupilDetailsList TranslatePupilsFromDTO(List<PupilDTO> pupils, string schoolName, short keyStage)
        {
            var pupilList = new PupilDetailsList();

            pupilList.AddRange(pupils.Select(x => TranslatePupilFromDTO(x, schoolName, keyStage)));

            return pupilList;
        }

        public static string TranslatePupilJSONResultFromDTO(PupilDTO pupil, string schoolName, short keyStage)
        {
            return GetJson(TranslatePupilV2FromDTO(pupil, schoolName, keyStage));
        }
    }
}