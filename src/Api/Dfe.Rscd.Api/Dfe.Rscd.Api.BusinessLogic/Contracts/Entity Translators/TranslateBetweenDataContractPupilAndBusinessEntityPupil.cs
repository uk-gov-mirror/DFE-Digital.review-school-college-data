using System;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.TransactionScripts;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;
using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractPupilAndBusinessEntityPupil
    {

        public const int DATA_ORIGIN_ID_USER_ADDITION = 3;

        public static Pupil TranslateBusinessEntityPupilToDataContractPupil(Students pupilIn)
        {
            //*****************************************************************************************
            // LATEST INFO WHERE DATE END IS NULL
            //*****************************************************************************************
            var studentChangeLatest = pupilIn.StudentChanges.Where(r => r.DateEnd == null).FirstOrDefault();

            var outPupil = new Pupil {AdmissionDateDisplayString = string.Empty};

            if (studentChangeLatest.ENTRYDAT.Length == 8)
            {
                DateTime? admissionDate = TSStudent.TryConvertDateTimeDBString(studentChangeLatest.ENTRYDAT);
                if (admissionDate.HasValue)
                {
                    outPupil.AdmissionDate = admissionDate.Value;
                }

                outPupil.AdmissionDateDisplayString = studentChangeLatest.ENTRYDAT.ToShortDateTimeString();
            }
            outPupil.Forename = studentChangeLatest.Forename;
            outPupil.ForvusIndex = pupilIn.ForvusIndex.Value;
            outPupil.Surname = studentChangeLatest.Surname;

            outPupil.DOBDisplayString = "";
            if (studentChangeLatest.DOB.Length == 8)
            {
                DateTime? dob = TSStudent.TryConvertDateTimeDBString(studentChangeLatest.DOB);
                if (dob.HasValue)
                {
                    outPupil.DOB = dob.Value;
                }

                outPupil.DOBDisplayString = studentChangeLatest.DOB.ToShortDateTimeString();
            }
            if (studentChangeLatest.Ethnicities != null)
            {
                outPupil.Ethnicity = new Ethnicity
                                         {
                                             Code = studentChangeLatest.Ethnicities.EthnicityCode,
                                             Description = studentChangeLatest.Ethnicities.EthnicityDescription
                                         };
            }

            outPupil.Age = studentChangeLatest.Age; // this should be calculated as in pupil list converter

            if (studentChangeLatest.Languages != null)
            {
                outPupil.FirstLanguage = new FirstLanguage
                                             {
                                                 Code = studentChangeLatest.Languages.LanguageCode,
                                                 Description = studentChangeLatest.Languages.LanguageDescription
                                             };
            }

            if (studentChangeLatest.FSM != null)
            {
                outPupil.FSM = new FSM {Description = studentChangeLatest.FSM};
            }

            if (studentChangeLatest.Gender != null)
            {
                outPupil.Gender = new Gender {Description = studentChangeLatest.Gender};
            }

            if (studentChangeLatest.LookedAfterEver != null)
            {
                if (studentChangeLatest.LookedAfterEver.Equals("0"))
                    outPupil.InCare = true;
                else
                    outPupil.InCare = false;
            }

            if (studentChangeLatest.SENStatus != null)
            {
                outPupil.SENStatus = new DataContracts.SENStatus
                                         {
                                             Code = Convert.ToChar(studentChangeLatest.SENStatus.SENStatusCode),
                                             Description = studentChangeLatest.SENStatus.SENStatusDescription
                                         };
            }


            outPupil.StudentID = studentChangeLatest.StudentID;

            if (studentChangeLatest.YearGroups != null)
            {
                outPupil.YearGroup = studentChangeLatest.YearGroups.YearGroupDescription;
            }

            if (pupilIn.PINCLs != null)
            {
                outPupil.PINCL = new DataContracts.PINCLs { DisplayFlag = pupilIn.PINCLs.DisplayFlag, P_INCL = pupilIn.PINCLs.P_INCL, P_INCLDescription= pupilIn.PINCLs.P_INCLDescription};
            }

            /*
             * AcceptanceStatus = 
               case isnull(ScrutinyStatusCode,'no')
                  when 'no' then 'Accepted'
                  when 'P%' then 'Pending'
                -- when 'W' then ScrutinyStatusCode
                  when 'A%' then 'Accepted'
                  when 'R' then ScrutinyStatusCode
            end,
             */
            if (pupilIn.StudentRequests.FirstOrDefault() != null && pupilIn.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault() !=null)
            {
                if (pupilIn.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault().ScrutinyStatus != null)
                {
                    ScrutinyStatus ss = pupilIn.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault().ScrutinyStatus;

                    if (ss.ScrutinyStatusDescription.StartsWith("A"))
                        outPupil.ScrutinyStatusText = "Accepted";
                    else if (ss.ScrutinyStatusDescription.StartsWith("P"))
                        outPupil.ScrutinyStatusText = "Pending";
                    else if (ss.ScrutinyStatusDescription.StartsWith("W"))
                        outPupil.ScrutinyStatusText = ss.ScrutinyStatusDescription;
                    else if (ss.ScrutinyStatusDescription.StartsWith("R"))
                        outPupil.ScrutinyStatusText = ss.ScrutinyStatusDescription;
                }
                else                
                    outPupil.ScrutinyStatusText = "Accepted";
            }

            //*****************************************************************************************
            // ORIGINAL UNAMENDED INFO WITH LEAST CHANGEID
            //*****************************************************************************************
            var studentChangeOld = pupilIn.StudentChanges.Where(r => r.DateEnd != null).OrderByDescending(r=>r.ChangeID).FirstOrDefault();

            if (studentChangeOld != null)
            {
                outPupil.OldForename = studentChangeOld.Forename;
                outPupil.OldSurname = studentChangeOld.Surname;

                if (studentChangeOld.DOB.Length == 8)
                {
                    outPupil.OldDOB = new DateTime(
                        int.Parse(studentChangeOld.DOB.Substring(0, 4)),
                        int.Parse(studentChangeOld.DOB.Substring(4, 2)),
                        int.Parse(studentChangeOld.DOB.Substring(6, 2)));
                    outPupil.OldDOBDisplayString = outPupil.OldDOB.Value.ToShortDateString();
                }

                if (studentChangeOld.ENTRYDAT.Length == 8)
                {
                    outPupil.OldAdmissionDate = new DateTime(
                        int.Parse(studentChangeOld.ENTRYDAT.Substring(0, 4)),
                        int.Parse(studentChangeOld.ENTRYDAT.Substring(4, 2)),
                        int.Parse(studentChangeOld.ENTRYDAT.Substring(6, 2)));
                    outPupil.OldAdmissionDateDisplayString = outPupil.OldAdmissionDate.Value.ToShortDateString();
                }

                if (studentChangeOld.Gender != null)
                {
                    outPupil.OldGender = studentChangeOld.Gender;
                }

                if (studentChangeOld.Ethnicities != null)
                {
                    outPupil.OldEthnicity = new Ethnicity
                                                {
                                                    Code = studentChangeOld.Ethnicities.EthnicityCode,
                                                    Description = studentChangeOld.Ethnicities.EthnicityDescription
                                                };
                }

                outPupil.Age = studentChangeLatest.Age;

                if (studentChangeOld.Languages != null)
                {
                    outPupil.OldFirstLanguage = new FirstLanguage
                                                    {
                                                        Code = studentChangeOld.Languages.LanguageCode,
                                                        Description = studentChangeOld.Languages.LanguageDescription
                                                    };
                }

                if (studentChangeOld.YearGroups != null)
                {
                    outPupil.OldYearGroup = studentChangeOld.YearGroups.YearGroupDescription;
                }
            }
            else // same as the latest one
            {
                outPupil.OldForename = studentChangeLatest.Forename;
                outPupil.OldSurname = studentChangeLatest.Surname;

                if (studentChangeLatest.DOB.Length == 8)
                {
                    outPupil.OldDOB = new DateTime(
                        int.Parse(studentChangeLatest.DOB.Substring(0, 4)),
                        int.Parse(studentChangeLatest.DOB.Substring(4, 2)),
                        int.Parse(studentChangeLatest.DOB.Substring(6, 2)));
                    outPupil.OldDOBDisplayString = outPupil.OldDOB.Value.ToShortDateString();
                }

                if (studentChangeLatest.ENTRYDAT.Length == 8)
                {
                    outPupil.OldAdmissionDate = new DateTime(
                        int.Parse(studentChangeLatest.ENTRYDAT.Substring(0, 4)),
                        int.Parse(studentChangeLatest.ENTRYDAT.Substring(4, 2)),
                        int.Parse(studentChangeLatest.ENTRYDAT.Substring(6, 2)));
                    outPupil.OldAdmissionDateDisplayString = outPupil.OldAdmissionDate.Value.ToShortDateString();
                }

                if (studentChangeLatest.Gender != null)
                {
                    outPupil.OldGender = studentChangeLatest.Gender;
                }

                if (studentChangeLatest.Ethnicities != null)
                {
                    outPupil.OldEthnicity = new Ethnicity
                                                {
                                                    Code = studentChangeLatest.Ethnicities.EthnicityCode,
                                                    Description = studentChangeLatest.Ethnicities.EthnicityDescription
                                                };
                }

                outPupil.Age = studentChangeLatest.Age;

                if (studentChangeLatest.Languages != null)
                {
                    outPupil.OldFirstLanguage = new FirstLanguage
                                                    {
                                                        Code = studentChangeLatest.Languages.LanguageCode,
                                                        Description = studentChangeLatest.Languages.LanguageDescription
                                                    };
                }

                if (studentChangeLatest.YearGroups != null)
                {
                    outPupil.OldYearGroup = studentChangeLatest.YearGroups.YearGroupDescription;
                }
            }

            return outPupil;
        }

        public static Students TranslateDataContractPupilToBusinessEntityStudent(PupilDetails pupilIn)
        {
            return TranslateDataContractPupilToBusinessEntityStudent(pupilIn, false);
        }

        public static Students TranslateDataContractPupilToBusinessEntityStudent(PupilDetails pupilIn, bool IsForPupilAdd)
        {
            var studentOut = new Students();
            studentOut.StudentID = pupilIn.PupilID;

            studentOut.Schools = new Checking.DataAccess.Schools {DFESNumber = pupilIn.SchoolDFESNumber};
            studentOut.Cohorts = new Checking.DataAccess.Cohorts {KeyStage = pupilIn.KeyStage};
            studentOut.ForvusIndex = pupilIn.ForvusNumber;
            studentOut.MATCHREF = null;
            studentOut.SERAPNumberOfEntries = 0;
            studentOut.PINCLs = new Checking.DataAccess.PINCLs();

            if (IsForPupilAdd)
            {
                //Determine a new PINCL value for this pupil.
                string pinclCode;
                switch (pupilIn.KeyStage)
                {
                    case (2):
                        pinclCode = "299";
                        break;
                    case (4):
                        pinclCode = (TSSchool.IsSchoolNonPlasc(pupilIn.SchoolDFESNumber)) ? "498" : "499";
                        break;
                    case (5):
                        pinclCode = "599";
                        break;
                    default:
                        throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKeyStageForPupilAdd);
                }

                studentOut.PINCLs.P_INCL = pinclCode;

                if (pupilIn.OriginalPupilID != null)
                    studentOut.OriginalStudentID = pupilIn.OriginalPupilID.Value;

            }
            else
            {
                //Set the PINCL code.
                if (pupilIn.PINCLCode != null) studentOut.PINCLs.P_INCL = pupilIn.PINCLCode;
            }

            studentOut.DataOrigin = new DataOrigin {DataOriginID = DATA_ORIGIN_ID_USER_ADDITION};
            studentOut.PortlandStudentID = null;

            var studentChange = TranslateDataContractPupilToBusinessEntityStudentChange(pupilIn);
            studentOut.StudentChanges.Add(studentChange);
                

            return studentOut;
        }

        private static StudentChanges TranslateDataContractPupilToBusinessEntityStudentChange(PupilDetails pupilIn)
        {
            var studentChangeOut = new StudentChanges
                                       {
                                           StudentID = pupilIn.PupilID,
                                           Surname = pupilIn.Surname,
                                           Forename = pupilIn.Forename,
                                           Gender = pupilIn.Gender.ToString(),
                                           DOB = pupilIn.DOBDisplayString.ToDBDateTimeString(),
                                           UPN = pupilIn.UPN
                                       };

            DateTime dob;
            if (DateTime.TryParse(pupilIn.DOBDisplayString, out dob))
                studentChangeOut.Age = (byte)TSStudent.CalculateStudentAge(dob);
            else
                studentChangeOut.Age = (byte)pupilIn.Age.GetValueOrDefault();

            if(!String.IsNullOrEmpty(pupilIn.PostCode)) 
                studentChangeOut.PostCode = pupilIn.PostCode;
            else
                studentChangeOut.PostCode = "";

            studentChangeOut.ENTRYDAT = pupilIn.AdmissionDateDisplayString.ToDBDateTimeString();

            if(pupilIn.YearGroup != null)
                studentChangeOut.YearGroups = new YearGroups { YearGroupCode = pupilIn.YearGroup };

            if(pupilIn.EthnicityCode != null)
                studentChangeOut.Ethnicities = new Ethnicities { EthnicityCode = pupilIn.EthnicityCode };

            studentChangeOut.FSM = (pupilIn.FreeSchoolMeals) ? "1" : "0";
            
            if(pupilIn.FirstLanguageCode != null)
                studentChangeOut.Languages = new Languages { LanguageCode = pupilIn.FirstLanguageCode};

            studentChangeOut.PostCode = pupilIn.PostCode ?? string.Empty;

            if(pupilIn.SENStatusCode != null)
                studentChangeOut.SENStatus = new Checking.DataAccess.SENStatus { SENStatusCode = pupilIn.SENStatusCode };

            studentChangeOut.LookedAfterEver = (pupilIn.InCare) ? "1" : "0";
            
            if(pupilIn.NORFLAGE.HasValue) studentChangeOut.NORFLAGE = new NORFLAGE { NORFLAGE1 = pupilIn.NORFLAGE.Value };

            studentChangeOut.DateEnd = null;

            return studentChangeOut;

        }
    }
}