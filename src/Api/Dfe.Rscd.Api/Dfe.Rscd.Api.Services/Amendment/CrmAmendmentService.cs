using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Dfe.Rscd.Api.Services
{
    public class CrmAmendmentService : IAmendmentService
    {
        private readonly IEnumerable<IAmendmentBuilder> _amendmentBuilders;
        private readonly IEstablishmentService _establishmentService;
        private readonly IOrganizationService _organizationService;
        private readonly Guid _sharePointDocumentLocationRecordId;
        private IAmendmentBuilder CurrentBuilder { get; set; }

        public CrmAmendmentService(
            IOrganizationService organizationService,
            IEstablishmentService establishmentService,
            DynamicsOptions dynamicsOptions,
            IEnumerable<IAmendmentBuilder> amendmentBuilders)
        {
            _establishmentService = establishmentService;
            _amendmentBuilders = amendmentBuilders;
            _organizationService = organizationService;
            _sharePointDocumentLocationRecordId = dynamicsOptions.SharePointDocumentLocationRecordId;
        }

        public Amendment GetAmendment(CheckingWindow checkingWindow, string id)
        {
            return GetAmendment(checkingWindow, new Guid(id));
        }

        public IEnumerable<Amendment> GetAmendments(CheckingWindow checkingWindow, string urn)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendments = context.rscd_AmendmentSet.Where(a => a.rscd_URN == urn).ToList();

                return amendments.Where(a => ValidAmendmentForCheckingWindow(checkingWindow, a))
                    .OrderByDescending(o => o.CreatedOn)
                    .Select(x => Convert(checkingWindow, x));
            }
        }

        public AmendmentOutcome AddAmendment(Amendment amendment)
        {
            CurrentBuilder = RetrieveBuilderForAmendment(amendment.AmendmentType);

            var outcome = CurrentBuilder.BuildAmendments(amendment);

            if (outcome.IsComplete && outcome.IsAmendmentCreated)
            {
                var amendmentEstablishment = GetOrCreateEstablishment(amendment.CheckingWindow, amendment.URN);
                RelateEstablishmentToAmendment(amendmentEstablishment, outcome.NewAmendmentId);

                // RelateEvidence
                if (outcome.EvidenceStatus == EvidenceStatus.Now)
                    RelateEvidence(outcome.NewAmendmentId, amendment.EvidenceFolderName, false);

                outcome.NewAmendmentReferenceNumber = GetAmendment(amendment.CheckingWindow, outcome.NewAmendmentId).Reference;
            }

            return outcome;
        }

        public void RelateEvidence(string id, string evidenceFolderName, bool updateEvidenceOption)
        {
            RelateEvidence(new Guid(id), evidenceFolderName, updateEvidenceOption);
        }

        public bool CancelAmendment(string id)
        {
            var cols = new ColumnSet("rscd_outcome", "statecode");
            var amendment =
                (rscd_Amendment) _organizationService.Retrieve(rscd_Amendment.EntityLogicalName, new Guid(id), cols);

            if (amendment == null || amendment.rscd_Outcome == rscd_Outcome.Cancelled) return false;

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

            var fetchXml = @"<fetch version='1.0'>
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
                var fetchRequest1 = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                var returnCollection =
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
                                expandoDict[attribute.Value] = optionsSets[attribute.Key]
                                    .First(t => t.Item1 == ((OptionSetValue) recordAttributeValue).Value).Item2;
                            else
                                expandoDict[attribute.Value] = recordAttributeValue.ToString();
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

        private IAmendmentBuilder RetrieveBuilderForAmendment(AmendmentType amendmentType)
        {
            return _amendmentBuilders.FirstOrDefault(x => x.AmendmentType == amendmentType);
        }

        private IAmendmentBuilder RetrieveBuilderForAmendment(rscd_Amendmenttype? amendmentType)
        {
            return _amendmentBuilders.FirstOrDefault(x => x.CrmAmendmentType == amendmentType);
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
            CurrentBuilder = RetrieveBuilderForAmendment(amendment.rscd_Amendmenttype);
                
            var amendmentDomain = CurrentBuilder.CreateAmendment();
            
            amendmentDomain.Id = amendment.Id.ToString();
            amendmentDomain.CheckingWindow = amendment.rscd_Checkingwindow.Value.ToDomainCheckingWindow();
            amendmentDomain.Reference = amendment.rscd_Referencenumber;
            amendmentDomain.URN = amendment.rscd_URN;
            amendmentDomain.Pupil = new Pupil
            {
                Forename = amendment.rscd_Firstname,
                Surname = amendment.rscd_Lastname,
                Gender = amendment.rscd_Gender.Value.ToDomainGenderType(),
                DOB = amendment.rscd_Dateofbirth.Value,
                Age = amendment.rscd_Age.HasValue ? amendment.rscd_Age.Value : 0,
                AdmissionDate = amendment.rscd_Dateofadmission.GetValueOrDefault(),
                YearGroup = amendment.rscd_Yeargroup,
                UPN = amendment.rscd_UPN,
                ULN = amendment.rscd_ULN
            };
            amendmentDomain.EvidenceStatus = amendment.rscd_Evidencestatus.Value.ToDomainEvidenceStatus();
            amendmentDomain.CreatedDate = amendment.CreatedOn.Value;
            amendmentDomain.Status = GetStatus(checkingWindow, amendment);
            
            amendmentDomain.AmendmentDetail = GetAmendmentDetails(amendment);

            return amendmentDomain;
        }

        private string GetStatus(CheckingWindow checkingWindow, rscd_Amendment amendment)
        {
            // TODO: Branching on checking window is ugly - we need to refactor this class
            if (checkingWindow == CheckingWindow.KS4Late
                && amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4June)
                switch (amendment.rscd_Outcome)
                {
                    case rscd_Outcome.AwaitingDfEreview:
                    case rscd_Outcome.AwaitingReviewer2:
                    case rscd_Outcome.AwaitingReviewer3:

                        return "Requested";

                    default:

                        return amendment.rscd_Outcome.ToString();
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

        private AmendmentDetail GetAmendmentDetails(rscd_Amendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                return CurrentBuilder.CreateAmendmentDetails(context, amendment);
            }
        }

        private bool ValidAmendmentForCheckingWindow(CheckingWindow checkingWindow, rscd_Amendment amendment)
        {
            switch (checkingWindow)
            {
                case CheckingWindow.KS4June:
                    return amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4June;
                case CheckingWindow.KS4Late:
                    return amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4June &&
                           new[]
                           {
                               rscd_Outcome.Approvedandfinal,
                               rscd_Outcome.Rejectedandfinal,
                               rscd_Outcome.Autoapproved,
                               rscd_Outcome.Autorejected
                           }.Any(s =>
                               s == amendment.rscd_Outcome) ||
                           amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS4Late;
                case CheckingWindow.KS5:
                    return amendment.rscd_Checkingwindow == rscd_Checkingwindow.KS5;
            }

            return false;
        }

        private cr3d5_establishment GetOrCreateEstablishment(CheckingWindow checkingWindow, string id)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var establishmentDto =
                    context.cr3d5_establishmentSet.SingleOrDefault(
                        e => e.cr3d5_Urn == id);
                if (establishmentDto == null)
                    establishmentDto =
                        context.cr3d5_establishmentSet.SingleOrDefault(
                            e => e.cr3d5_LAEstab == id);

                if (establishmentDto != null) return establishmentDto;

                School establishment = null;
                try
                {
                    establishment = _establishmentService.GetByURN(checkingWindow, new URN(id));
                }
                catch
                {
                }

                if (establishment == null) establishment = _establishmentService.GetByDFESNumber(checkingWindow, id);

                if (establishment == null) return null;


                establishmentDto = new cr3d5_establishment
                {
                    cr3d5_name = establishment.SchoolName,
                    cr3d5_Urn = establishment.Urn.Value,
                    cr3d5_LAEstab = establishment.DfesNumber.ToString(),
                    cr3d5_Schooltype = establishment.SchoolType,
                    cr3d5_Numberofamendments = 0
                };
                context.AddObject(establishmentDto);
                var result = context.SaveChanges();
                if (result.HasError)
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();

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

        private void RelateEvidence(Guid amendmentId, string evidenceFolderName, bool updateEvidenceOption)
        {
            // https://community.dynamics.com/crm/f/microsoft-dynamics-crm-forum/203503/adding-a-sharepointdocumentlocation-programmatically/528485
            var sharepointdocumentlocation = new Entity("sharepointdocumentlocation");
            sharepointdocumentlocation["name"] = evidenceFolderName;
            sharepointdocumentlocation["description"] = amendmentId.ToString();

            // TODO: Currently hard-coded to a document location record that points to the "Amendment" sub-folder. Will need to decide
            // on final folder structure for organising file uploads - possibly have a folder per checking window ({year}/{checking-window})
            sharepointdocumentlocation["parentsiteorlocation"] = new EntityReference(
                "sharepointdocumentlocation", _sharePointDocumentLocationRecordId);
            sharepointdocumentlocation["relativeurl"] = evidenceFolderName;
            sharepointdocumentlocation["regardingobjectid"] =
                new EntityReference(rscd_Amendment.EntityLogicalName, amendmentId);

            var sharepointdocumentlocationid = _organizationService.Create(sharepointdocumentlocation);

            if (updateEvidenceOption) UpdateEvidenceStatus(amendmentId); //TODO: this needs fixing for amendment view
        }

        private bool UpdateEvidenceStatus(Guid amendmentId)
        {
            var cols = new ColumnSet("rscd_evidencestatus", "rscd_outcome", "statecode");
            var amendment =
                (rscd_Amendment) _organizationService.Retrieve(rscd_Amendment.EntityLogicalName, amendmentId, cols);

            if (amendment == null || amendment.rscd_Evidencestatus == rscd_Evidencestatus.Now) return false;

            amendment.rscd_Evidencestatus = rscd_Evidencestatus.Now;
            amendment.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
            amendment.StateCode = rscd_AmendmentState.Active;

            _organizationService.Update(amendment);
            return true;
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
            var stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);

            var doc = new XmlDocument();
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
            var attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                var pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            var pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            var countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);

            var sb = new StringBuilder(1024);
            var stringWriter = new StringWriter(sb);

            var writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }
}