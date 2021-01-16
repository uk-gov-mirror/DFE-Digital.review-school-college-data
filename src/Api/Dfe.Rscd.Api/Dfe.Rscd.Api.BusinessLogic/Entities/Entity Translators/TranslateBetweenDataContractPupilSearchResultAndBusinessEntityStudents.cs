using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.TransactionScripts;
using Web09.Checking.DataAccess;
using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractPupilDetailsAndBusinessEntityStudents
    {

        public static PupilDetailsList TranslateBusinessEntityStudentListToDataContractPupilDetailsList(List<Web09.Checking.DataAccess.Students> pupilsIn)
        {
            var pupilsOut = new Web09.Services.DataContracts.PupilDetailsList();
            pupilsOut.AddRange(pupilsIn.ConvertAll(r =>
                TranslateBetweenDataContractPupilDetailsAndBusinessEntityStudents
                    .TranslateBusinessEntityStudentToDataContractPupilDetails(r)));
            return pupilsOut;
        }

        public static PupilDetails TranslateBusinessEntityStudentToDataContractPupilDetails(Web09.Checking.DataAccess.Students pupilIn)
        {
            return TranslateBusinessEntityStudentToDataContractPupilDetails(pupilIn, true);
        }

        public static PupilDetails TranslateBusinessEntityStudentToDataContractPupilDetails(Web09.Checking.Business.Logic.Entities.CohortDetailPupil pupilIn)
        {
            PupilDetails pupilOut = new PupilDetails();
            
            // re-use existing translator without expensive data
            pupilOut = TranslateBusinessEntityStudentToDataContractPupilDetails(pupilIn.Student, false);

            // populate additional fields
            pupilOut.HasResultAmendments = pupilIn.HasResultAmendments;
            pupilOut.KS1EXP              = pupilIn.KS1EXP;
            pupilOut.NewMobile           = pupilIn.NewMobile;

            return pupilOut;
        }

        public static PupilDetails TranslateBusinessEntityStudentToDataContractPupilDetails(Web09.Checking.DataAccess.Students pupilIn, bool loadExpensiveData)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            PupilDetails pupilOut = new PupilDetails();
            pupilOut.PupilID = pupilIn.StudentID;

            if (pupilIn.ForvusIndex.HasValue) pupilOut.ForvusNumber = pupilIn.ForvusIndex.Value;

            StudentChanges studentChangeLatest = pupilIn.StudentChanges.First();
            pupilOut.Forename = textInfo.ToTitleCase(textInfo.ToLower(studentChangeLatest.Forename));
            pupilOut.Surname = textInfo.ToTitleCase(textInfo.ToLower(studentChangeLatest.Surname));
            pupilOut.UPN = studentChangeLatest.UPN;

            //For conversion of any dates, ensure that the stored value
            //is a legitimate date string (ie yyyymmdd).
            int numericDate;
            if (Int32.TryParse(studentChangeLatest.DOB, out numericDate) && numericDate >= 10000000 && numericDate <= 99999999)
            {
                DateTime? dobIn = TSStudent.TryConvertDateTimeDBString(studentChangeLatest.DOB);
                if (dobIn.HasValue)
                {
                    pupilOut.DOB = dobIn;
                }

                pupilOut.DOBDisplayString = studentChangeLatest.DOB.ToShortDateTimeString();
            }
            else
            {
                pupilOut.DOB = null;
                pupilOut.DOBDisplayString = "";
            }
            
            pupilOut.Age = studentChangeLatest.Age;

            //Defensive cast of DB admissiondate string to date time value
            DateTime? admissionDateDT = TSStudent.TryConvertDateTimeDBString(studentChangeLatest.ENTRYDAT);
            if (admissionDateDT.HasValue)
            {
                pupilOut.AdmissionDate = admissionDateDT.Value;
                pupilOut.AdmissionDateDisplayString = admissionDateDT.Value.ToShortDateString();
            }
            else
            {
                pupilOut.AdmissionDate = null;
                pupilOut.AdmissionDateDisplayString = studentChangeLatest.ENTRYDAT.ToShortDateTimeString();
            }
            
            pupilOut.Gender = Convert.ToChar(studentChangeLatest.Gender);
            
            // PARENTCODE WILL ALWAYS BE PARENT CODE EVEN FOR BOTH CHILD AND PARENT 
            pupilOut.EthnicityCode = studentChangeLatest.Ethnicities.ParentEthnicityCode;

            pupilOut.YearGroup = studentChangeLatest.YearGroups.YearGroupCode;
            if(studentChangeLatest.Languages != null) pupilOut.FirstLanguageCode = studentChangeLatest.Languages.LanguageCode;
            if(studentChangeLatest.NORFLAGE != null) pupilOut.NORFLAGE = studentChangeLatest.NORFLAGE.NORFLAGE1;
            if(studentChangeLatest.SENStatus != null) pupilOut.SENStatusCode = studentChangeLatest.SENStatus.SENStatusCode;
            pupilOut.KeyStage = pupilIn.Cohorts.KeyStage;
            pupilOut.SchoolDFESNumber = pupilIn.Schools.DFESNumber;
            pupilOut.SchoolName = pupilIn.Schools.SchoolName;
            if (pupilIn.PINCLs != null)
            {
                if(!string.IsNullOrEmpty(pupilIn.PINCLs.DisplayFlag))
                    pupilOut.IsIncluded = Convert.ToChar(pupilIn.PINCLs.DisplayFlag);

                pupilOut.PINCLDescription = pupilIn.PINCLs.P_INCLDescription;
                pupilOut.PINCLCode        = pupilIn.PINCLs.P_INCL;
                pupilOut.PINCLDisplayFlag = pupilIn.PINCLs.DisplayFlag;
            }

            pupilOut.PostCode       = studentChangeLatest.PostCode;
            pupilOut.LatestChangeID = studentChangeLatest.ChangeID;            
            
            pupilOut.FreeSchoolMeals = (studentChangeLatest.FSM=="1"? true: false);
            pupilOut.InCare          = (studentChangeLatest.LookedAfterEver == "1" ? true : false);

            // Required for Cohort Details only if its the latest change
            pupilOut.ScrutinyRequestStatus="";            
            if (studentChangeLatest.DateEnd==null && pupilIn.StudentRequests.FirstOrDefault() != null && pupilIn.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault() != null)
            {
                if (pupilIn.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault().ScrutinyStatus != null)
                {
                    ScrutinyStatus ss = pupilIn.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault().ScrutinyStatus;

                    if (ss.ScrutinyStatusDescription.StartsWith("A"))
                        pupilOut.ScrutinyRequestStatus = "Accepted";
                    else if (ss.ScrutinyStatusDescription.StartsWith("P"))
                        pupilOut.ScrutinyRequestStatus = "Pending";
                    else if (ss.ScrutinyStatusDescription.StartsWith("W"))
                        pupilOut.ScrutinyRequestStatus = ss.ScrutinyStatusDescription;
                    else if (ss.ScrutinyStatusDescription.StartsWith("R"))
                        pupilOut.ScrutinyRequestStatus = ss.ScrutinyStatusDescription;
                }
                else
                    pupilOut.ScrutinyRequestStatus = "Accepted";
            }
           
            if (loadExpensiveData)
            {
                bool hasResultAmendments = false;
                bool hasOutstandingRequest = false;
                String idaciValue = "";

                TSStudent.LoadStudentInfoForTranslator(pupilIn.StudentID, studentChangeLatest.PostCode, ref idaciValue, ref hasResultAmendments, ref hasOutstandingRequest);
                pupilOut.IDACIValue            = idaciValue;
                pupilOut.HasResultAmendments   = hasResultAmendments;
                pupilOut.HasOutstandingRequest = hasOutstandingRequest;
            }

            return pupilOut;
        }

        public static CohortDetailsSortOptions TranslateDataContractCohortDetailsSortEnumToCohortDetailsSortOptions(CohortDetailsSortEnum searchEnumIn)
        {
            CohortDetailsSortOptions searchEnumOut = (CohortDetailsSortOptions)(Enum.Parse(typeof(CohortDetailsSortOptions), searchEnumIn.ToString()));
            return searchEnumOut;
        }
    }
}
