using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class CrmAmendmentService : IAmendmentService
    {
        private readonly IOrganizationService _organizationService;
        private IEstablishmentService _establishmentService;
        private readonly Guid _sharePointDocumentLocationRecordId;
        private IAmendmentBuilder _amendmentBuilder;

        public CrmAmendmentService(
            IOrganizationService organizationService,
            IEstablishmentService establishmentService,
            IOptions<DynamicsOptions> dynamicsOptions,
            IAmendmentBuilder amendmentBuilder)
        {
            _amendmentBuilder = amendmentBuilder;
            _establishmentService = establishmentService;
            _organizationService = organizationService;
            _sharePointDocumentLocationRecordId = dynamicsOptions.Value.SharePointDocumentLocationRecordId;
        }

        public Amendment GetAmendment(CheckingWindow checkingWindow, string id)
        {
            return GetAmendment(checkingWindow, new Guid(id));
        }

        private Amendment GetAmendment(CheckingWindow checkingWindow, Guid amendmentId)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.rscd_AmendmentSet.FirstOrDefault(
                    x => x.Id == amendmentId);

                return amendment != null ? Convert(checkingWindow, amendment) : null;
            }
        }

        private Amendment Convert(CheckingWindow checkingWindow, rscd_Amendment amendment)
        {
            return new Amendment
            {
                Id = amendment.Id.ToString(),
                CheckingWindow = amendment.rscd_Checkingwindow.Value.ToDomainCheckingWindow(),
                AmendmentType = amendment.rscd_Amendmenttype.Value.ToDomainAmendmentType(),
                Reference = amendment.rscd_Referencenumber,
                URN = amendment.rscd_URN,
                Pupil = new Pupil
                {
                    ForeName = amendment.rscd_Firstname,
                    LastName = amendment.rscd_Lastname,
                    Gender = amendment.rscd_Gender.Value.ToDomainGenderType(),
                    DateOfBirth = amendment.rscd_Dateofbirth.Value,
                    Age = amendment.rscd_Age.HasValue ? amendment.rscd_Age.Value : 0,
                    DateOfAdmission = amendment.rscd_Dateofadmission.GetValueOrDefault(),
                    YearGroup = amendment.rscd_Yeargroup,
                    UPN = amendment.rscd_UPN,
                    ULN = amendment.rscd_ULN
                },
                EvidenceStatus = amendment.rscd_Evidencestatus.Value.ToDomainEvidenceStatus(),
                CreatedDate = amendment.CreatedOn.Value,
                Status = GetStatus(checkingWindow, amendment),
                AmendmentDetail = GetAdmendmentDetails(amendment)
            };
        }

        // TODO: API should return an enum for status
        private string GetStatus(CheckingWindow checkingWindow, rscd_Amendment amendment)
        {
            // TODO: Branching on checking window is ugly - we need to refactor this class
            if (checkingWindow == CheckingWindow.KS4Late 
                && amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4June)
            {
                switch (amendment.rscd_Outcome)
                {
                    case rscd_Outcome.AwaitingDfEreview:
                    case rscd_Outcome.AwaitingReviewer2:
                    case rscd_Outcome.AwaitingReviewer3:

                        return "Requested";

                    default:

                        return amendment.rscd_Outcome.ToString();
                }

            }

            switch (amendment.rscd_Outcome)
            {
                case rscd_Outcome.Awaitingevidence:
                case rscd_Outcome.Cancelled:

                    return amendment.rscd_Outcome.ToString();

                default:

                    return "Requested";
            }
        }

        private IAmendmentType GetAdmendmentDetails(rscd_Amendment amendment)
        {
            if (amendment.rscd_Amendmenttype == rscd_Amendmenttype.Removeapupil)
            {
                using (var context = new CrmServiceContext(_organizationService))
                {
                    if (amendment.rscd_Removepupil != null)
                    {
                        var removePupil = context.rscd_RemovepupilSet.FirstOrDefault(x => x.Id == amendment.rscd_Removepupil.Id);
                        return new RemovePupil
                        {
                            Reason = removePupil.rscd_Reason,
                            SubReason = removePupil.rscd_Subreason,
                            Detail = removePupil.rscd_Details,
                            AllocationYear = removePupil.rscd_Allocationyear
                        };

                    }
                    return new RemovePupil();
                }
            }
            else
            {
                using (var context = new CrmServiceContext(_organizationService))
                {
                    var addPupil = context.rscd_AddpupilSet
                        .Where(x => x.Id == amendment.rscd_Addpupil.Id).First();
                    return new AddPupil
                    {
                        Reason = addPupil.rscd_Reason.Value.ToDomainAddReason(),
                        PreviousSchoolLAEstab = addPupil.rscd_PreviousschoolLAESTAB,
                        PreviousSchoolURN = addPupil.rscd_PreviousschoolURN,
                        PriorAttainmentResults = new List<PriorAttainment>
                        {
                            new PriorAttainment {Subject = Ks2Subject.Reading, ExamYear = addPupil.rscd_Readingexamyear, TestMark = addPupil.rscd_Readingexammark, ScaledScore = addPupil.rscd_Readingscaledscore},
                            new PriorAttainment {Subject = Ks2Subject.Writing, ExamYear = addPupil.rscd_Writingexamyear, TestMark = addPupil.rscd_Writingteacherassessment, ScaledScore = addPupil.rscd_Writingscaledscore},
                            new PriorAttainment {Subject = Ks2Subject.Maths, ExamYear = addPupil.rscd_Mathsexamyear, TestMark = addPupil.rscd_Mathsexammark, ScaledScore = addPupil.rscd_Mathsscaledscore}
                        }
                    };
                }
            }
        }

        public IEnumerable<Amendment> GetAmendments(CheckingWindow checkingWindow, string urn)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendments = context.rscd_AmendmentSet.Where(a => a.rscd_URN == urn).ToList();

                return amendments.Where(a => ValidAmendmentForCheckingWindow(checkingWindow, a))
                    .OrderByDescending(o => o.CreatedOn)
                    //.Skip(0)
                    //.Take(30)
                    .Select(x => Convert(checkingWindow, x));
            }
        }

        private bool ValidAmendmentForCheckingWindow(CheckingWindow checkingWindow, rscd_Amendment amendment)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS4June:
                    return amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4June;
                case CheckingWindow.KS4Late:
                    return (amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4June &&
                            new[] { rscd_Outcome.Approvedandfinal, 
                                    rscd_Outcome.Rejectedandfinal,
                                    rscd_Outcome.Autoapproved,
                                    rscd_Outcome.Autorejected}.Any(s =>
                                s == amendment.rscd_Outcome)) ||
                           amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4Late;
                case CheckingWindow.KS5:
                    return amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS5;
            }

            return false;
        }

        public string CreateAmendment(Amendment amendment)
        {
            var amendmentId = _amendmentBuilder.BuildAmendments(amendment);

            // Relate to establishment
            var amendmentEstablishment = GetOrCreateEstablishment(amendment.CheckingWindow, amendment.URN);
            RelateEstablishmentToAmendment(amendmentEstablishment, amendmentId);

            // RelateEvidence
            if (amendment.EvidenceStatus == EvidenceStatus.Now)
            {
                RelateEvidence(amendmentId, amendment.EvidenceFolderName, false);
            }

            return GetAmendment(amendment.CheckingWindow, amendmentId).Reference;
        }

        private cr3d5_establishment GetOrCreateEstablishment(CheckingWindow checkingWindow, string id)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var establishmentDto =
                    context.cr3d5_establishmentSet.SingleOrDefault(
                        e => e.cr3d5_Urn == id);
                if (establishmentDto == null)
                {
                    establishmentDto =
                        context.cr3d5_establishmentSet.SingleOrDefault(
                            e => e.cr3d5_LAEstab == id);
                }

                if (establishmentDto != null)
                {
                    return establishmentDto;
                }

                Establishment establishment = null;
                try
                {
                    establishment = _establishmentService.GetByURN(checkingWindow, new URN(id));
                }
                catch
                {
                }

                if (establishment == null)
                {
                    establishment = _establishmentService.GetByLAId(checkingWindow, id);
                }

                if (establishment == null)
                {
                    return null;
                }


                establishmentDto = new cr3d5_establishment
                {
                    cr3d5_name = establishment.Name,
                    cr3d5_Urn = establishment.Urn.Value,
                    cr3d5_LAEstab = establishment.LaEstab,
                    cr3d5_Schooltype = establishment.SchoolType,
                    cr3d5_Numberofamendments = 0
                };
                context.AddObject(establishmentDto);
                var result = context.SaveChanges();
                if (result.HasError)
                {
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
                }

                return establishmentDto;
            }
        }

        private void RelateEstablishmentToAmendment(cr3d5_establishment establishment, Guid amendmentId)
        {
            var relationship = new Relationship("rscd_cr3d5_establishment_rscd_amendments");
            _organizationService.Associate(cr3d5_establishment.EntityLogicalName, establishment.Id, relationship,
                new EntityReferenceCollection
                {
                    new EntityReference(rscd_Amendment.EntityLogicalName, amendmentId)
                });
        }

        public void RelateEvidence(string id, string evidenceFolderName, bool updateEvidenceOption)
        {
            RelateEvidence(new Guid(id), evidenceFolderName, updateEvidenceOption);
        }

        private void RelateEvidence(Guid amendmentId, string evidenceFolderName, bool updateEvidenceOption)
        {
            // https://community.dynamics.com/crm/f/microsoft-dynamics-crm-forum/203503/adding-a-sharepointdocumentlocation-programmatically/528485
            Entity sharepointdocumentlocation = new Entity("sharepointdocumentlocation");
            sharepointdocumentlocation["name"] = evidenceFolderName;
            sharepointdocumentlocation["description"] = amendmentId.ToString();

            // TODO: Currently hard-coded to a document location record that points to the "Amendment" sub-folder. Will need to decide
            // on final folder structure for organising file uploads - possibly have a folder per checking window ({year}/{checking-window})
            sharepointdocumentlocation["parentsiteorlocation"] = new EntityReference(
                "sharepointdocumentlocation", _sharePointDocumentLocationRecordId);
            sharepointdocumentlocation["relativeurl"] = evidenceFolderName;
            sharepointdocumentlocation["regardingobjectid"] = new EntityReference(rscd_Amendment.EntityLogicalName, amendmentId);

            Guid sharepointdocumentlocationid = _organizationService.Create(sharepointdocumentlocation);

            if (updateEvidenceOption)
            {
                UpdateEvidenceStatus(amendmentId); //TODO: this needs fixing for amendment view
            }
        }

        private bool UpdateEvidenceStatus(Guid amendmentId)
        {
            var cols = new ColumnSet("rscd_evidencestatus", "rscd_outcome");
            var amendment =
                (rscd_Amendment) _organizationService.Retrieve(rscd_Amendment.EntityLogicalName, amendmentId, cols);

            if (amendment == null || amendment.rscd_Evidencestatus == rscd_Evidencestatus.Now)
            {
                return false;
            }

            amendment.rscd_Evidencestatus = rscd_Evidencestatus.Now;
            amendment.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;

            _organizationService.Update(amendment);
            return true;
        }

        public bool CancelAmendment(string id)
        {
            var cols = new ColumnSet("rscd_outcome", "statecode");
            var amendment = (rscd_Amendment)_organizationService.Retrieve(rscd_Amendment.EntityLogicalName, new Guid(id), cols);

            if (amendment == null || amendment.rscd_Outcome == rscd_Outcome.Cancelled)
            {
                return false;
            }

            amendment.rscd_Outcome = rscd_Outcome.Cancelled;
            amendment.StateCode = rscd_AmendmentState.Inactive;

            _organizationService.Update(amendment);
            return true;
        }

        public IEnumerable<IDictionary<string, object>> GetAmendments()
        {
            var fetchCount = 50;
            var pageNumber = 1;
            var recordCount = 0;

            // Specify the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            // Reference: https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/org-service/page-large-result-sets-with-fetchxml
            // Only retrieving the attributes that are needed
            // Currently this only filters on Final decision == Approved
            // For Beta more filter criteria would be needed, depending on how we define the data model to support the different key stages
            // and types of amendment.
            var attributeDictionary = new Dictionary<string, string>
            {
                {"new_amendmentid", "AMENDMENT_ID"},
                {"rscd_amendmenttype", "AMENDMENT_TYPE"},
                {"cr3d5_addreasontype", "ADD_PUPIL_REASON"},
                {"cr3d5_urn", "URN"},
                {"cr3d5_laestab", "LAESTAB"},
                {"cr3d5_pupilid", "PUPIL_ID"},
                {"cr3d5_forename", "FORENAME"},
                {"cr3d5_surname", "SURNAME"},
                {"cr3d5_dob", "DATE_OF_BIRTH"},
                {"cr3d5_gender", "GENDER"},
                {"cr3d5_admissiondate", "ADMISSION_DATE"},
                {"cr3d5_yeargroup", "YEAR_GROUP"},
                {"cr3d5_includeinperformanceresults", "INCL_IN_PERF_RESULTS"},
                {"cr3d5_addpupilref", "REFERENCE_NUMBER"},
                {"rscd_readingexamyear", "PA_READING_EXAM_YEAR"},
                {"rscd_readingtestmark", "PA_READING_TEST_MARK"},
                {"rscd_readingscaledscore", "PA_READING_SCALED_SCORE"},
                {"rscd_writingexamyear", "PA_WRITING_EXAM_YEAR"},
                {"rscd_writingtestmark", "PA_WRITING_TEST_MARK"},
                {"rscd_writingscaledscore", "PA_WRITING_SCALED_SCORE"},
                {"rscd_mathsexamyear", "PA_MATHS_EXAM_YEAR"},
                {"rscd_mathstestmark", "PA_MATHS_TEST_MARK"},
                {"rscd_mathsscaledscore", "PA_MATHS_SCALED_SCORE"}
            };

            string fetchXml = @"<fetch version='1.0'>
                                  <entity name='new_amendment'>
                                    <filter>
                                        <condition attribute='cr3d5_finaldecisionname' operator= 'eq' value='Approved' />
                                    </filter>
                                    <order attribute='new_amendmentid' />
                                  </entity>
                               </fetch>";

            Directory.CreateDirectory("output");

            // Build fetchXml XmlDoc with specified attributes.
            var xmlDoc = CreateXmlDoc(fetchXml, attributeDictionary.Keys.ToArray());

            var optionsSets = GetOptionSets();
            var amendments = new List<IDictionary<string, object>>();

            while (true)
            {
                // update fetchXml with new paging values
                var xml = CreateXml(xmlDoc, pagingCookie, pageNumber, fetchCount);

                // Execute the fetch query and get the xml result.
                RetrieveMultipleRequest fetchRequest1 = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                EntityCollection returnCollection =
                    ((RetrieveMultipleResponse) _organizationService.Execute(fetchRequest1)).EntityCollection;

                recordCount += returnCollection.Entities.Count;

                foreach (var c in returnCollection.Entities)
                {
                    // Unfortunately, if a record doesn't have a value for a requested attribute, the attribute is not returned for that
                    // record. This makes things a bit trickier when writing to CSV as you can end up with values written against the wrong
                    // columns.
                    dynamic expandoObj = new ExpandoObject();
                    var expandoDict = (IDictionary<string, object>) expandoObj;

                    foreach (var attribute in attributeDictionary)
                    {
                        var attributeFound = c.TryGetAttributeValue(attribute.Key, out object recordAttributeValue);

                        if (attributeFound)
                        {
                            if (recordAttributeValue is OptionSetValue)
                            {
                                expandoDict[attribute.Value] = optionsSets[attribute.Key]
                                    .First(t => t.Item1 == ((OptionSetValue) recordAttributeValue).Value).Item2;
                            }
                            else
                            {
                                expandoDict[attribute.Value] = recordAttributeValue.ToString();
                            }
                        }
                        else
                        {
                            expandoDict[attribute.Value] = "";
                        }
                    }

                    amendments.Add(expandoObj);
                }

                // Check for more records, if it returns 1.
                if (returnCollection.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.                            
                    pagingCookie = returnCollection.PagingCookie;
                }
                else
                {
                    // If no more records in the result nodes, exit the loop.
                    break;
                }
            }

            return amendments;
        }

        private Dictionary<string, List<Tuple<int, string>>> GetOptionSets()
        {
            var attributes = new[] {"rscd_amendmenttype", "cr3d5_addreasontype", "cr3d5_gender"};
            var optionSetsLookup = new Dictionary<string, List<Tuple<int, string>>>();
            var attrReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = "new_amendment",
                RetrieveAsIfPublished = true
            };
            foreach (var attribute in attributes)
            {
                attrReq.LogicalName = attribute;
                var attrResponse = (RetrieveAttributeResponse) _organizationService.Execute(attrReq);
                var attrMetaData = (EnumAttributeMetadata) attrResponse.AttributeMetadata;
                var lookup = attrMetaData.OptionSet.Options
                    .Select(o => new Tuple<int, string>(o.Value.Value, o.Label.UserLocalizedLabel.Label)).ToList();
                optionSetsLookup.Add(attribute, lookup);
            }

            return optionSetsLookup;
        }

        private static XmlDocument CreateXmlDoc(string xml, string[] attributes)
        {
            StringReader stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);

            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            var entityNode = doc.DocumentElement.SelectSingleNode("/fetch/entity");

            foreach (var attribute in attributes)
            {
                var attNode = doc.CreateElement("attribute");

                attNode.SetAttribute("name", attribute);
                entityNode.AppendChild(attNode);
            }

            return doc;
        }

        private static string CreateXml(XmlDocument doc, string cookie, int page, int count)
        {
            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }
}
