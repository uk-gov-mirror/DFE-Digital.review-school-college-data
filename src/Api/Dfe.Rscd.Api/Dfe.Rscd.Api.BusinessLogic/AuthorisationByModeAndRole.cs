using System;
using System.Collections;
using System.Collections.Generic;
using Web09.Common.Roles;
using Web09.Services.Common;

namespace Web09.Checking.AuthorisationRules
{
    public static class AuthorisationByModeAndRole
    {
        private class RoleSet
        {
            public bool SuperUser { get; set; }
            public bool DA { get; set; }
            public bool DFE { get; set; }
            public bool Helpline { get; set; }
            public bool SeniorHelpLine { get; set; }
            public bool InstitutionHead { get; set; }
            public bool InstitutionAdmin { get; set; }
            public bool InstitutionUser { get; set; }
            public bool InstitutionDFE { get; set; }
            public bool LAHead { get; set; }
            public bool LA { get; set; }
            public bool ScrutinyLogging { get; set; }
            public bool ScrutinyResultsOnly { get; set; }
            public bool ScrutinyStandard { get; set; }
            public bool ScrutinyExpert { get; set; }

            public RoleSet()
            {
                SuperUser = false;
                DA = false;
                DFE = false;
                Helpline = false;
                SeniorHelpLine = false;
                InstitutionHead = false;
                InstitutionAdmin = false;
                InstitutionUser = false;
                InstitutionDFE = false;
                LAHead = false;
                LA = false;
                ScrutinyLogging = false;
                ScrutinyResultsOnly = false;
                ScrutinyStandard = false;
                ScrutinyExpert = false;
            }

            public bool Matches(string roleName)
            {
                bool matches = (SuperUser && roleName.Equals(Web09Roles.WEB09_SUPERUSER, StringComparison.CurrentCultureIgnoreCase))                   
                    || (DFE && roleName.Equals(Web09Roles.WEB09_DCSF, StringComparison.CurrentCultureIgnoreCase))                  
                    || (SeniorHelpLine && roleName.Equals(Web09Roles.WEB09_HELP_SENIOR, StringComparison.CurrentCultureIgnoreCase))
                    || (InstitutionHead && roleName.Equals(Web09Roles.WEB09_INSTITUTION_HEAD, StringComparison.CurrentCultureIgnoreCase))
                    || (InstitutionAdmin && roleName.Equals(Web09Roles.WEB09_INSTITUTION_ADMIN, StringComparison.CurrentCultureIgnoreCase))
                    || (InstitutionUser && roleName.Equals(Web09Roles.WEB09_INSTITUTION_USER, StringComparison.CurrentCultureIgnoreCase))
                    || (InstitutionDFE && roleName.Equals(Web09Roles.WEB09_INSTITUTION_DCSF, StringComparison.CurrentCultureIgnoreCase))
                    || (LAHead && roleName.Equals(Web09Roles.WEB09_LA_HEAD, StringComparison.CurrentCultureIgnoreCase))
                    || (LA && roleName.Equals(Web09Roles.WEB09_LA, StringComparison.CurrentCultureIgnoreCase))                  
                    || (ScrutinyExpert && roleName.Equals(Web09Roles.WEB09_SCRUTINY_EXPERT, StringComparison.CurrentCultureIgnoreCase))
                    ;

                return matches;
            }
        }

        public static bool IsRequestedPageAuthorisedForAnonymousAccess(string loginurl, string requestedURL)
        {
            bool isAuthorisedForAnonymousAccess = false;

            if ( (requestedURL.IndexOf(loginurl, StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("contactus", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("termsandconditions", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("faqview", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("aatchecking_privacy", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("aatchecking_securityadvice", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("aatchecking_terms", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("guidance", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("loginhelp", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("newusershelp", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("existingusershelp", StringComparison.CurrentCultureIgnoreCase) > -1
                    || requestedURL.IndexOf("documents", StringComparison.CurrentCultureIgnoreCase) > -1))
            {
                isAuthorisedForAnonymousAccess = true;
            }

            return isAuthorisedForAnonymousAccess;
        }
        

        #region SchoolGroup Parsing
        public static bool IsPLASCSchool(ArrayList schoolGroups)
        {
            bool isPlasc = false;

            foreach (string schoolGroupName in schoolGroups)
            {
                if (schoolGroupName.Equals("PLASC", StringComparison.CurrentCultureIgnoreCase))
                {
                    isPlasc = true;
                }
            }

            return isPlasc;
        }


        public static bool IsIndependantSchool(ArrayList schoolGroups)
        {
            bool isIndependant = false;

            foreach (string schoolGroupName in schoolGroups)
            {
                if (schoolGroupName.Equals("Independent", StringComparison.CurrentCultureIgnoreCase))
                {
                    isIndependant = true;
                }
            }

            return isIndependant;
        }

        public static bool IsFESchool(ArrayList schoolGroups)
        {
            bool isFE = false;

            foreach (string schoolGroupName in schoolGroups)
            {
                if (schoolGroupName.Equals("FE", StringComparison.CurrentCultureIgnoreCase))
                {
                    isFE = true;
                }
            }

            return isFE;
        }

        public static bool IsMaintainedSchool(ArrayList schoolGroups)
        {
            bool isMaintained = false;

            foreach (string schoolGroupName in schoolGroups)
            {
                if (schoolGroupName.Equals("Maintained", StringComparison.CurrentCultureIgnoreCase))
                {
                    isMaintained = true;
                }
            }

            return isMaintained;
        }
        #endregion

        #region Page Level Authorisation

        public static bool IsLevel3DataSharingWebsite( string WebSiteType )
        {
            return WebSiteType.Equals("Level3DataSharing", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsOptinWebsite( string WebSiteType )
        {

            return WebSiteType.Equals("Optin", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsPasswordsLetterWebsite( string WebSiteType )
        {
            return WebSiteType.Equals("PasswordsLetter", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsDestinationsWebsite( string WebSiteType )
        {
            return ((WebSiteType.Equals("Destinations", StringComparison.CurrentCultureIgnoreCase)) ||
                (WebSiteType.Equals("DestinationsWithQSRData", StringComparison.CurrentCultureIgnoreCase)));
        }

        public static bool IsQSRWebsite
            (string WebSiteType)
        {
            return WebSiteType.Equals("DestinationsWithQSRData", StringComparison.CurrentCultureIgnoreCase);
        }
      
        public static bool CanOptin(string roleName)
        {
            return roleName.Equals("Web09_Institution_Head", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool CanLogon(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = true;

            if ( IsOptinWebsite( websiteType ) )
            {
                authorised = new RoleSet
                {
                    SuperUser = true,
                    DA = true,
                    SeniorHelpLine = true,
                    InstitutionHead = true,
                    InstitutionAdmin = true,
                    InstitutionDFE = true,
                    InstitutionUser = true,     
                    ScrutinyStandard = true,
                    ScrutinyExpert = true,
                    DFE = true,
                    Helpline = true,
                    LA = false,
                    LAHead = false,
                    ScrutinyLogging = true,
                    ScrutinyResultsOnly = true
                }.Matches(roleName);
                return authorised;
            }

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:

                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = false,
                        InstitutionAdmin = false,
                        InstitutionDFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true,
                        DFE = false,
                        Helpline = true,
                        LA = false,
                        LAHead = false, 
                        ScrutinyLogging = false,
                        ScrutinyResultsOnly = false
                    }.Matches(roleName);

                    break;
                default:
                    authorised = true;
                    break;
            }
            
            return authorised;
        }

        public static bool CanConfirmDataStatus(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = true;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;
                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = false;
                    break;
                default:
                    authorised = true;
                    break;
            }

            return authorised;
        }

        public static bool CanDoBackGroundAndHistoricalDataChecking(string websiteType, string websiteMode, string roleName)
        {
            return false;
        }

        public static bool CanDoAdminTasks(string websiteType, string websiteMode, string roleName)
        {
            bool forbidden = new RoleSet
            {
                InstitutionUser = true
            }.Matches(roleName);

            return !forbidden && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanViewReports(string websiteType, string websiteMode, string roleName)
        {
            return false;
        }

        public static bool CanManageMessages(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = new RoleSet
            {
                SuperUser = true
            }.Matches(roleName);

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanEditConfiguration(string websiteType, string websiteMode, string roleName)
        {
            return false;
        }

        public static bool CanManageHelpText(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = new RoleSet
            {
                SuperUser = true
            }.Matches(roleName);

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanChangeSchool(string websiteType, string websiteMode, string roleName)
        {
            bool forbidden = new RoleSet
            {
                InstitutionHead = true,
                InstitutionAdmin = true,
                InstitutionUser = true,
                ScrutinyLogging = true
            }.Matches(roleName);

            return !forbidden && !IsPasswordsLetterWebsite(websiteType);
        }

        public static bool CanEditPupil(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanIncludeRemovePupils(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanAddPupils(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanViewPupil(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LA = true,
                        LAHead = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }     

        public static bool CanViewSummaryPage(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            if (IsOptinWebsite(websiteType))
            {
                authorised = new RoleSet
                {
                    SuperUser = true,
                    DA = true,
                    Helpline = true,
                    SeniorHelpLine = true,
                    ScrutinyStandard = true,
                    ScrutinyExpert = true,
                    DFE = true,
                    InstitutionAdmin = true,
                    InstitutionDFE = true,
                    InstitutionHead = true,
                    InstitutionUser = true,
                    ScrutinyLogging = true,
                    ScrutinyResultsOnly = true,
                    LA = false,
                    LAHead = false
                }.Matches(roleName);

                return authorised && !IsLevel3DataSharingWebsite(websiteType);
            }
                     

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;              

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                // TFS 19338
                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true                         
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanEditConfirmationPage(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,                      
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanViewConfirmationPage(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName); 
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanViewDecisionsPage(string websiteType, string websiteMode, string roleName, ArrayList schoolGroups, int schoolLowestAge)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);
            bool isMaintainedSchool = IsMaintainedSchool(schoolGroups);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = (isMaintainedSchool && (schoolLowestAge < 16));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = ( isMaintainedSchool && (schoolLowestAge < 16) );
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true,
                        SeniorHelpLine = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanDisagreeOnLateResults(string websiteType, string websiteMode, string roleName, int selectedKeyStage)
        {          
            bool authorised = false;

            if (CanViewLateResults(websiteType, websiteMode, roleName))
            {
                if ( !WebSiteModeHelper.IsKS2Website(websiteMode) && selectedKeyStage == 5 )
                {
                    authorised = true;
                }
            }                    

            return authorised;
        }

        public static bool CanViewLateResults(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;
            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
                  
                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanEditLateResults(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;
            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = false;
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanManageEvidence(string websiteType, string websiteMode, string roleName)
        {
            return CanEditResults( websiteType, websiteMode, roleName );
        }


        /*
         * There was some old code AccessHelper.UserCanEdit() which said user can edit regardless of websitemode and rolename]
         * If they were an LA user and the school was in one of the groups : APSchools, PRUSchools, or its a closed school
         * Need to check with Peter if this logic should be added here.
         */
        public static bool CanEditResults(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;
            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true                       
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanAddResults(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;
            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanViewResults(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;
            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        InstitutionHead = true,
                        InstitutionAdmin = true,
                        InstitutionUser = true,
                        InstitutionDFE = true,
                        LAHead = true,
                        LA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true
                    }.Matches(roleName);
                    break;


                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        Helpline = true,
                        SeniorHelpLine = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }


            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }


        // TFS 20370
        public static bool CanEditResultsScrutiny(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }


        public static bool CanOpenResultsScrutiny(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    //SD 32124
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }


        // TFS 20369
        public static bool CanEditCohortScrutiny(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyLogging = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyLogging = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanOpenCohortScrutiny(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyLogging = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyLogging = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        DFE = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = new RoleSet
                    {
                        SuperUser = true,
                        DA = true,
                        ScrutinyStandard = true,
                        ScrutinyExpert = true
                    }.Matches(roleName);
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanViewFaqs(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = false;
                    break;
            }

            return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
        }

        public static bool CanOpenCohortPerformance(string websiteType, string websiteMode, string roleName)
        {
            {
                bool authorised = false;

                WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

                switch (mode)
                {
                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                        authorised = false;
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                        authorised = false;
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                            ScrutinyResultsOnly = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                            ScrutinyResultsOnly = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                        authorised = false;
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                        authorised = false;
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                        authorised = false;
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;
                }

                return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
            }
        }

        public static bool CanManageFAQ(string websiteType, string websiteMode, string roleName)
        {
            return false;
        }
        
        
        public static bool CanManageUsers(string websiteType, string websiteMode, string roleName)
        {
            {
                bool authorised = false;

                if (IsOptinWebsite(websiteType))
                {
                    return new RoleSet { 
                        DA = true,
                        Helpline = true ,
                        ScrutinyExpert = true,
                        ScrutinyLogging = true,
                        ScrutinyResultsOnly = true,
                        ScrutinyStandard = true,
                        SeniorHelpLine = true,
                        SuperUser = true
                    }.Matches(roleName);
                }

                WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);                           
                
                switch (mode)
                {
                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyExpert = true
                        }.Matches(roleName);
                        break;
                }
                //SD KS5 QSR Data 
                return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsOptinWebsite(websiteType);
            }
        }

        public static bool CanOpenHomePage(string websiteType, string websiteMode, string roleName)
        {
            {              
                bool authorised = false;
                         
                WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

                switch (mode)
                {
                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            // SD TFS 23124
                            ScrutinyLogging = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            // SD TFS 23124
                            ScrutinyLogging = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            // SD TFS 23124
                            ScrutinyLogging = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            // SD TFS 23124
                            ScrutinyLogging = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            // SD TFS 23124
                            ScrutinyLogging = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;
                }

                return authorised && !IsPasswordsLetterWebsite(websiteType);
            }
        }

        public static bool CanViewPublicDocuments(string websiteType, string websiteMode)
        {
            return (!IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsOptinWebsite(websiteType));
        }

        public static bool CanViewOptin(string websiteType, string websiteMode, string roleName)
        {
            return IsOptinWebsite(websiteType);
        }

        public static bool CanViewDocuments(string websiteType, string websiteMode, string roleName)
        {
            {
                if (IsOptinWebsite(websiteType))
                {
                    return true;
                }

                bool authorised = false;

                WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

                switch (mode)
                {
                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyResultsOnly = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            DFE = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            InstitutionHead = true,
                            InstitutionAdmin = true,
                            InstitutionUser = true,
                            InstitutionDFE = true,
                            LAHead = true,
                            LA = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;

                    case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                        authorised = new RoleSet
                        {
                            SuperUser = true,
                            DA = true,
                            Helpline = true,
                            SeniorHelpLine = true,
                            ScrutinyStandard = true,
                            ScrutinyExpert = true,
                        }.Matches(roleName);
                        break;
                }

                return authorised && !IsPasswordsLetterWebsite(websiteType) && !IsDestinationsWebsite(websiteType) && !IsLevel3DataSharingWebsite(websiteType);
            }
        }

       
        #endregion

        #region ColumnLevelAuthorisation

       
        
      
        public static bool CanViewEthnicityColumn(string websiteMode, string roleName, ArrayList schoolGroups )
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = false;
                    break;
            }

            return authorised;
        }

        public static bool CanViewFSMColumn(string websiteMode, string roleName, ArrayList schoolGroups)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }

        /// <summary>
        /// CLA = Child Looked After. This is referred to as InCare in some parts of code.
        /// </summary>
        /// <param name="websiteMode"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool CanViewCLAColumn(string websiteMode, string roleName, ArrayList schoolGroups )
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking: case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }

        /// <summary>
        /// KS1EXP is called Prior Attainment
        /// </summary>
        /// <param name="websiteMode"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool CanViewKS1EXPColumn(string websiteMode, string roleName, ArrayList schoolGroups )
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }

        /// <summary>
        /// NewMobile is called Mobility
        /// </summary>
        /// <param name="websiteMode"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool CanViewNewMobileColumn(string websiteMode, string roleName, ArrayList schoolGroups )
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }

        public static bool CanViewForvusIDColumn(string websiteMode, string roleName)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }

        /// <summary>
        /// NewMobile is called Mobility
        /// </summary>
        /// <param name="websiteMode"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool CanViewSenColumn(string websiteMode, string roleName, ArrayList schoolGroups)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }              
       
        public static bool CanViewAdoptedFromCareColumn(string websiteMode, string roleName, ArrayList schoolGroups, int tableYear)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = IsPLASCSchool(schoolGroups);
                    break;
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = (tableYear > 2015) && (IsPLASCSchool(schoolGroups));
                    break;
            }

            return authorised;
        }

        /// <summary>
        /// NewMobile is called Mobility
        /// </summary>
        /// <param name="websiteMode"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool CanViewFirstLangColumn(string websiteMode, string roleName, ArrayList schoolGroups)
        {
            bool authorised = false;

            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = !(IsIndependantSchool(schoolGroups) || IsFESchool(schoolGroups));
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = true;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = true;
                    break;
            }

            return authorised;
        }        

        public static bool CanViewSchoolDestinations(string websiteType, string websiteMode, string roleName)
        {
            bool authorised = false;


            WebSiteModeHelper.WebsiteModeEnum mode = WebSiteModeHelper.GetWebSiteModeEnum(websiteMode);

            switch (mode)
            {
                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilChecking:
                    authorised = IsDestinationsWebsite(websiteType);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS4PupilClosed:
                    authorised = IsDestinationsWebsite(websiteType);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Checking:
                    authorised = IsDestinationsWebsite(websiteType);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Closed:
                    authorised = IsDestinationsWebsite(websiteType);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45CheckedData:
                    authorised = IsDestinationsWebsite(websiteType);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS45Errata:
                    authorised = IsDestinationsWebsite(websiteType);
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Checking:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Closed:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2CheckedData:
                    authorised = false;
                    break;

                case WebSiteModeHelper.WebsiteModeEnum.KS2Errata:
                    authorised = false;
                    break;
            }


            return authorised;
        }

       
        public static bool CanViewP8SummaryPage(string WebSiteType, string WebSiteMode, string roleName, ArrayList schoolGroups, int institutionType, int schoolLowestAge, int schoolHighestAge)
        {
            if ( WebSiteModeHelper.IsKS2Website(WebSiteMode) )
            {
                return false;
            }

            List<int> authorisedInstitutionTypes = new List<int> { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 48, 50, 51, 52, 53, 55, 57, 58 };
            if (!authorisedInstitutionTypes.Contains(institutionType))
            {
                return false;
            }

            if (schoolLowestAge == 16 && schoolHighestAge == 18)
            {
                return false;
            }
                   
            return true;
        }
        
       
        public static string GetHomePageForWebsiteType( string WebSiteType )
        {
            string homePage = string.Empty;

            if (WebSiteType.Equals("Standard", StringComparison.CurrentCultureIgnoreCase))
            {
                homePage = "home.aspx";
            }

            if (WebSiteType.Equals("Optin", StringComparison.CurrentCultureIgnoreCase))
            {
                homePage = "home.aspx";
            }

            if (WebSiteType.Equals("Progress8", StringComparison.CurrentCultureIgnoreCase))
            {
                homePage = "home.aspx";
            }

            if (WebSiteType.Equals("Destinations", StringComparison.CurrentCultureIgnoreCase))
            {
                homePage = "schoolDestinations.aspx";
            }

            if (WebSiteType.Equals("DestinationsWithQSRData", StringComparison.CurrentCultureIgnoreCase))
            {
                homePage = "schoolDestinations.aspx";
            }           

            if (WebSiteType.Equals("Level3DataSharing", StringComparison.CurrentCultureIgnoreCase))
            {
                homePage = "Level3VA.aspx";
            }


            return homePage;
        }

        #endregion
    }
}
