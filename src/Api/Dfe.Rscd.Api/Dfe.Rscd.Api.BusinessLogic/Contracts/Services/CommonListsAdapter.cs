using System;
using System.Collections.Generic;
using System.Diagnostics;
using Web09.Checking.Business.Logic.TransactionScripts;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;
using Web09.Services.Adapters.EntityTranslators;
using Web09.Services.Common.JSONObjects;
using Web09.Services.DataContracts;
using Web09.Services.MessageContracts;
using Web09.Services.ServiceContracts;
using Web09.Services.StubAdapters;
using Cohorts = Web09.Services.DataContracts.Cohorts;

namespace Web09.Services.Adapters
{
    public class CommonListsAdapter : IWeb09CommonLists
    {
        private const string modeKS2CheckedData = "KS2 Checked Data";
        private const string modeKS2Checking = "KS2 Checking";
        private const string modeKS2Closed = "KS2 Closed";
        private const string modeKS2Errata = "KS2 Errata";
        private const string modeKS4PupilChecking = "KS4 Pupil Checking";
        private const string modeKS4PupilClosed = "KS4 Pupil Closed";
        private const string modeKS45CheckedData = "KS4/5 Checked Data";
        private const string modeKS45Checking = "KS4/5 Checking";
        private const string modeKS45Closed = "KS4/5 Closed";
        private const string modeKS45Errata = "KS4/5 Errata";

        private CommonListResponse _responseMsg;

        #region IWeb09CommonLists Members

        public CommonListResponse GetEthnicityList()
        {
            try
            {

                _responseMsg = new CommonListResponse();
                IList<Ethnicities> ethnicityList = TSCommonLists.GetEthnicityList();
                _responseMsg.CommonList = TranslateBetweenDataControlCommonListsAndBusinessEntities
                    .TranslateBusinessEthnicityListToDataContractCommonList(ethnicityList as List<Ethnicities>);
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetLanguageList()
        {
            try
            {
                _responseMsg = new CommonListResponse();
                IList<Languages> languageList = TSCommonLists.GetLanguageList();
                _responseMsg.CommonList = TranslateBetweenDataControlCommonListsAndBusinessEntities
                    .TranslateBusinessLanguageListDoDataContractCommonList(languageList as List<Languages>);
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetSENList()
        {
            try
            {
                _responseMsg = new CommonListResponse();
                IList<Web09.Checking.DataAccess.SENStatus> senList = TSCommonLists.GetSENList();
                _responseMsg.CommonList = TranslateBetweenDataControlCommonListsAndBusinessEntities
                    .TranslateBusinessSENListDoDataContractCommonList(senList as List<Web09.Checking.DataAccess.SENStatus>);
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetKeyStageConfigurationResponse GetKeyStageConfigurationList(GetKeyStageConfigurationRequest request)
        {
            try
            {
                GetKeyStageConfigurationResponse response = new GetKeyStageConfigurationResponse();
                IList<CohortConfiguration> cohortConfigList = TSCommonLists.GetKeyStageConfiguration(request.KeyStage);
                response.KeyStageConfigurationList = TranslateBetweenDataContractKeyStageConfigAndBusinessEntityCohortConfig
                    .TranslateBusinessEntityCohortConfigListToDataContractKeyStageConfigList(cohortConfigList as List<CohortConfiguration>);
                return response;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetYearGroups()
        {
            try
            {
                _responseMsg = new CommonListResponse
                {
                    CommonList = DataBuilder.CreateListWithItems("2021,2020,2019,2018,2017,2016,2015")
                };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetScrutinyStatusList()
        {
            try
            {
                _responseMsg = new CommonListResponse
                {
                    CommonList = DataBuilder.CreateListWithItems("Accepted,Cancelled,Pending,Rejected,Awaiting Evidence")
                };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetCohortAdjustmentRequestStatusList()
        {
            try
            {
                _responseMsg = new CommonListResponse();
                _responseMsg.CommonList = new CommonList();

                _responseMsg.CommonList.Add(new CommonListItem { Key = "4", DisplayText = "All requests" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "1", DisplayText = "Undecided unreferred" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "2", DisplayText = "Referred requests undecided by DfE" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "3", DisplayText = "Referred requests decided by DfE" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "5", DisplayText = "Requests decided by DfE" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "6", DisplayText = "Requests decided by Forvus" });
                //_responseMsg.CommonList.Add(new CommonListItem { Key = "8", DisplayText = "Requests referred to Forvus Expert" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "7", DisplayText = "Requests updated since last viewed" });
                _responseMsg.CommonList.Add(new CommonListItem { Key = "9", DisplayText = "Automatically accepted" });
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetScrutinyAcceptanceReasons(GetScrutinyAcceptanceReasonsRequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse
                {
                    CommonList = DataBuilder.CreateListWithItems("Auto,Passed,Checked")
                };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetScrutinyAcceptanceReasonAmendCodes(GetScrutinyAcceptanceReasonAmendCodesRequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse
                {
                    CommonList = DataBuilder.CreateListWithItems("Auto,Passed,Checked")
                };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetScrutinyRejectionReasons(GetScrutinyRejectionReasonsRequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse
                {
                    CommonList = DataBuilder.CreateListWithItems("Left School After ASC,Illness,Home Tuition,Funding Followed,Special Needs,Pupil Not Known")
                };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetScrutinyAmendCodes()
        {
            try
            {
                _responseMsg = new CommonListResponse
                {
                    CommonList = DataBuilder.CreateListWithItems("A,AA,C,PE,PD")
                };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetValidExamYearsResponse GetValidExamYears(GetValidExamYearsRequest request)
        {
            GetValidExamYearsResponse response = new GetValidExamYearsResponse();
            response.ExamList = DataBuilder.CreateListWithItems("2021,2020,2019,2018,2017");
            return response;
        }

        public void SaveUserSurvey(SaveUserSurveyRequest request)
        {
            try
            {
                Debug.WriteLine("SaveUserSurvey");
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CheckUserSurveyResponse CheckUserSurvey(CheckUserSurveyRequest request)
        {
            try
            {

                return new CheckUserSurveyResponse
                {
                    CheckUserSurveyResult = true
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CreateEvidenceEntryResponse CreateEvidenceEntry()
        {
            try
            {

                return new CreateEvidenceEntryResponse
                {
                    Barcode = "1212131"
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetSchoolEthinicityList(GetSchoolEthinicityListRequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse();
                IList<Ethnicities> ethnicityList = TSCommonLists.GetEthnicityList();
                _responseMsg.CommonList = TranslateBetweenDataControlCommonListsAndBusinessEntities
                    .TranslateBusinessEthnicityListToDataContractCommonList(ethnicityList as List<Ethnicities>);
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetSchoolLanguageList(GetSchoolLanguageListRequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse();
                IList<Languages> languageList = TSCommonLists.GetLanguageList();
                _responseMsg.CommonList = TranslateBetweenDataControlCommonListsAndBusinessEntities
                    .TranslateBusinessLanguageListDoDataContractCommonList(languageList as List<Languages>);
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetSchoolYearGroupList(GetSchoolYearGroupRrequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse { CommonList = DataBuilder.CreateListWithItems("2021,2020,2019,2018,2017,2016,2015") };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetSchoolAgeList(GetSchoolAgeListRequest request)
        {
            try
            {
                _responseMsg = new CommonListResponse { CommonList = DataBuilder.CreateGenericList("18,17,16,15,14,13,12,10,9,8,7") };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public CommonListResponse GetResultScrutinyReasons()
        {
            try
            {
                _responseMsg = new CommonListResponse { CommonList = DataBuilder.CreateListWithItems("Auto,Passed,Checked") };
                return _responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        /// <summary>
        /// Translates Checking Exercise Parameters to Cohort Configuration list and calls TransactionScript
        /// </summary>
        /// <param name="request"></param>
        public void SaveKeystageConfigurationList(SaveKeystageConfigurationListRequest request)
        {
            try
            {
                Debug.WriteLine("Save Keystage Config");
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetPupilFilterListsResponse GetPupilFilterLists(GetPupilFilterListsRequest request)
        {
            try
            {
                return new GetPupilFilterListsResponse
                {
                    AgeList = DataBuilder.CreateGenericList("18,17,16,15,14,13,12,10,9,8,7"),
                    EthnicityList = GetEthnicityList().CommonList,
                    LanguageList = GetLanguageList().CommonList,
                    AwardingBodyList = DataBuilder.CreateAwardingBodyCollection(),
                    CohortList = new Cohorts
                    {
                        new Cohort{KeyStage = 4, KeyStageName = "KS4"},
                        new Cohort{KeyStage = 5, KeyStageName = "KS5"}
                    },
                    ConfigurationList = new KeyStageConfigurationList
                    {
                        new KeyStageConfiguration{ConfigurationCode = "TableYear", KeyStage = 4, ConfigurationDescription = "Table Year", ConfigurationValue = "2020"}
                    },
                    YearGroupList = DataBuilder.CreateListWithItems("2021,2020,2019,2018,2017,2016,2015"),
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetPupilFilterListsV2Response GetPupilFilterListsV2(GetPupilFilterListsRequest request)
        {
            try
            {
                var pupilFilters = new PupilFilters();

                // 1st resultset returned is AgeList
                pupilFilters.Ages.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterAge
                {
                    Age = "16",
                    AgeLabel = "16"
                });

                // 2nd resultset is AwardingBodyList
                pupilFilters.AwardingBodyCodes.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterAwardingBody
                {
                    AwardingBodyID = "01",
                    AwardingBodyCode = "AO 1"
                });

                // 3rd resultset is CohortList
                pupilFilters.Cohorts.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterCohort
                {
                        KeyStage = 4,
                        KeyStageName = "Cohort 1"
                });

                // 4th resultset is ConfigurationList
                pupilFilters.ConfigurationItems.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterConfigurationValue
                {
                    ConfigurationCode = "TableYear",
                    ConfigurationValue = "2020"
                });

                // 5th resultset is Ethnicity list
                pupilFilters.Ethnicitys.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterEthnicity
                {
                    EthnicityCode = "01",
                    EthnicityDescription = "Eth 1",
                    ParentEthnicityCode = "Eth Parent 1",
                });

                // 6th resultset is LanguageList
                pupilFilters.Languages.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterLanguage
                {
                    LanguageCode = "01",
                    LanguageDescription = "Lang 1",
                });

                // 7th Resultset is YearGroupList
                pupilFilters.YearGroups.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterYearGroup
                {
                    YearGroupCode = "01",
                    YearGroupDescription = "YearGroup 1",
                });

                return new GetPupilFilterListsV2Response { JSON = DataBuilder.GetJson(pupilFilters) };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public AddToUserPasswordLetterResponse AddToUserPasswordLetter(AddToUserPasswordLetterRequest request)
        {
            try
            {
                return new AddToUserPasswordLetterResponse
                {
                    UsernameQueued = true,
                    LastPrintDate = DateTime.Now,
                    LastQueuedDate = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public void SetUserPasswordPrintedStatus(SetUserPasswordPrintedStatusRequest request)
        {
            Debug.WriteLine("SetUserPass");
        }

        public GetPasswordLettersToSendResponse GetPasswordLettersToSend()
        {
            return new GetPasswordLettersToSendResponse { Users = new List<string>() };
        }

        public GetPasswordLetterDataResponse GetPasswordLetterData(GetPasswordLetterDataRequest request)
        {
            var response = new GetPasswordLetterDataResponse() { RequiredLetters = new PasswordLetterDataList() };
            return response;
        }

        public GetPasswordLetterCountResponse GetPasswordLetterCount(GetPasswordLetterCountRequest request)
        {
            return new GetPasswordLetterCountResponse()
            { Count = 3 };
        }

        public GetUserStatusResponse GetUserStatus(GetUserStatusRequest request)
        {
            return new GetUserStatusResponse() { JSON = DataBuilder.GetJson(new UserStatus
            {
                CreateDate = DateTime.Now.AddMonths(-1),
                IsLockedOut = false,
                LastLockedOutDate = DateTime.Now.AddYears(-1),
                LastLoginDate = DateTime.Now.AddDays(-1)
            }) };
        }

        public GetContentResponse GetContent(GetContentRequest request)
        {
            // TODO see how to fudge this content management system
            var contentHolder = new ContentHolder {Content = "Content "+request.ContentURI+" goes here."};
            contentHolder.Context = new Dictionary<string, string>();
            contentHolder.Context.Add("ContentExpiresDateTime", "2090-12-01 12:00:00");
            return new GetContentResponse { JSON = DataBuilder.GetJson(contentHolder) };
        }

        #endregion
    }
}
