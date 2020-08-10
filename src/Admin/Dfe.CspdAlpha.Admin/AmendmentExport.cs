using CsvHelper;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dfe.CspdAlpha.Admin
{
    public class AmendmentExport
    {
        public static void Export(
            Action<string> log,
            string cdsConnectionString)
        {
            var dynamicsConnString = cdsConnectionString;
            var cdsClient = new CdsServiceClient(dynamicsConnString);
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
                {"cr3d5_postcode", "POSTCODE"},
                {"cr3d5_includeinperformanceresults", "INCL_IN_PERF_RESULTS"},
                {"cr3d5_addpupilref", "REFERENCE_NUMBER"},
                {"cr3d5_priorattainmentresultfor", "PRIOR_ATTN_RESULT_FOR"},
                {"cr3d5_priorattainmenttest", "PRIOR_ATTN_TEST"},
                {"cr3d5_priorattainmentacademicyear", "PRIOR_ATTN_ACAMDEMIC_YEAR"},
                {"cr3d5_priorattainmentlevel", "PRIOR_ATTN_LEVEL"}
            };


            string fetchXml = @"<fetch version='1.0'>
                                  <entity name='new_amendment'>
                                    <filter>
                                        <condition attribute='cr3d5_finaldecisionname' operator= 'eq' value='Approved' />
                                    </filter>
                                    <order attribute='new_amendmentid' />
                                  </entity>
                               </fetch>";

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            log($"{nameof(AmendmentExport)} started");

            Directory.CreateDirectory("output");

            // Build fetchXml XmlDoc with specified attributes.
            var xmlDoc = CreateXmlDoc(fetchXml, attributeDictionary.Keys.ToArray());

            using (var writer = File.CreateText("output/amendments.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
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
                        ((RetrieveMultipleResponse) cdsClient.Execute(fetchRequest1)).EntityCollection;


                    var optionsSets = GetOptionSets(cdsClient);

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

                        csv.WriteRecord(expandoObj);
                        csv.NextRecord();
                    }

                    log($"Page {pageNumber} processed");

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
            }

            stopwatch.Stop();
            log($"{nameof(AmendmentExport)} finished in {stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds}s");
            log($"{recordCount} amendments exported");
        }

        private static Dictionary<string, List<Tuple<int, string>>> GetOptionSets(CdsServiceClient client)
        {
            var attributes = new[] { "rscd_amendmenttype", "cr3d5_addreasontype", "cr3d5_gender" };
            var optionSetsLookup = new Dictionary<string, List<Tuple<int, string>>>();
            var attrReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = "new_amendment",
                RetrieveAsIfPublished = true
            };
            foreach (var attribute in attributes)
            {
                attrReq.LogicalName = attribute;
                var attrResponse = (RetrieveAttributeResponse)client.Execute(attrReq);
                var attrMetaData = (EnumAttributeMetadata)attrResponse.AttributeMetadata;
                var lookup = attrMetaData.OptionSet.Options.Select(o => new Tuple<int, string>(o.Value.Value, o.Label.UserLocalizedLabel.Label)).ToList();
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
            pageAttr.Value = Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = Convert.ToString(count);
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
