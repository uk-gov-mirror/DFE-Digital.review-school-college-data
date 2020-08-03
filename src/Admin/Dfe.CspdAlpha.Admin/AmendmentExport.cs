using CsvHelper;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

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

            // Set the number of records per page to retrieve.
            int fetchCount = 50;

            // Initialize the page number.
            int pageNumber = 1;

            // Initialize the number of records.
            int recordCount = 0;

            // Specify the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            // Reference: https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/org-service/page-large-result-sets-with-fetchxml
            // Only retrieving the attributes that are needed
            // Currently this only filters on Final decision == Approved
            // For Beta more filter criteria would be needed, depending on how we define the data model to support the different key stages
            // and types of amendment.
            string[] attributes = new[] { 
                "cr3d5_priorattainmentresultfor", 
                "cr3d5_dob",
                "new_amendmentid",
                "cr3d5_forename",
                "cr3d5_includeinperformanceresults",
                "cr3d5_priorattainmentacademicyear",
                "cr3d5_priorattainmentlevel",
                "cr3d5_pupilid",
                "cr3d5_laestab",
                "cr3d5_surname",
                "cr3d5_priorattainmenttest",
                "cr3d5_admissiondate",
                "cr3d5_yeargroup",
                "cr3d5_gender",
                "cr3d5_urn",
                "cr3d5_addpupilref",
                "cr3d5_postcode"
            };

            // TODO: Build fetchXml from list of requested attributes.
            string fetchXml = @"<fetch version='1.0'>
                                  <entity name='new_amendment'>
                                    <attribute name='cr3d5_priorattainmentresultfor' />
                                    <attribute name='cr3d5_dob' />
                                    <attribute name='new_amendmentid' />
                                    <attribute name='cr3d5_forename' />
                                    <attribute name='cr3d5_includeinperformanceresults' />
                                    <attribute name='cr3d5_priorattainmentacademicyear' />
                                    <attribute name='cr3d5_priorattainmentlevel' />
                                    <attribute name='cr3d5_pupilid' />
                                    <attribute name='cr3d5_laestab' />
                                    <attribute name='cr3d5_surname' />
                                    <attribute name='cr3d5_priorattainmenttest' />
                                    <attribute name='cr3d5_admissiondate' />
                                    <attribute name='cr3d5_yeargroup' />
                                    <attribute name='cr3d5_gender' />
                                    <attribute name='cr3d5_urn' />
                                    <attribute name='cr3d5_addpupilref' />
                                    <attribute name='cr3d5_postcode' />
                                    <filter>
                                        <condition attribute='cr3d5_finaldecision' operator= 'eq' value='353910000' />
                                    </filter>
                                    <order attribute='new_amendmentid' />
                                  </entity>
                               </fetch>";

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            log($"{nameof(AmendmentExport)} started");

            Directory.CreateDirectory("output");
            var start = true;

            using (var writer = File.CreateText("output/amendments.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                while (true)
                {
                    // Build fetchXml string with the placeholders.
                    string xml = CreateXml(fetchXml, pagingCookie, pageNumber, fetchCount);

                    // Execute the fetch query and get the xml result.
                    RetrieveMultipleRequest fetchRequest1 = new RetrieveMultipleRequest
                    {
                        Query = new FetchExpression(xml)
                    };

                    EntityCollection returnCollection = ((RetrieveMultipleResponse)cdsClient.Execute(fetchRequest1)).EntityCollection;

                    recordCount += returnCollection.Entities.Count;

                    foreach (var c in returnCollection.Entities)
                    {
                        // Unfortunately, if a record doesn't have a value for a requested attribute, the attribute is not returned for that
                        // record. This makes things a bit trickier when writing to CSV as you can end up with values written against the wrong
                        // columns.
                        dynamic expandoObj = new ExpandoObject();
                        var expandoDict = (IDictionary<string, object>)expandoObj;

                        foreach (var attribute in attributes)
                        {
                            var attributeFound = c.TryGetAttributeValue(attribute, out object recordAttributeValue);

                            if (attributeFound)
                            {
                                expandoDict[attribute] = recordAttributeValue is OptionSetValue 
                                    ? ((OptionSetValue)recordAttributeValue).Value.ToString() : recordAttributeValue.ToString();
                            }
                            else
                            {
                                expandoDict[attribute] = "";
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

        private static string CreateXml(string xml, string cookie, int page, int count)
        {
            StringReader stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);

            // Load document
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            return CreateXml(doc, cookie, page, count);
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
