using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace Dfe.CspdAlpha.Admin
{
    public class AmendmentLookup
    {
        public IDictionary<string, dynamic> ExistingPupilLookup { get; }
        public IList<dynamic> NewPupilLookup { get; }

        public AmendmentLookup(Action<string> log, string amendmentCsvFilePath)
        {
            ExistingPupilLookup = new Dictionary<string, dynamic>();
            NewPupilLookup = new List<dynamic>();
            if (!string.IsNullOrEmpty(amendmentCsvFilePath))
            {
                using (var reader = new StreamReader(amendmentCsvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // TODO: Regex replace would probably be better here
                    csv.Configuration.PrepareHeaderForMatch = (s, i) =>
                        s.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).ToLower()
                            .Trim();
                    var records = csv.GetRecords<dynamic>();


                    foreach (var record in records)
                    {
                        var amendmentType = (string) record.amendment_type;
                        if (amendmentType.Equals("Remove pupil", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        var addPupilReason = (string) record.add_pupil_reason;
                        if (addPupilReason.Equals("New pupil"))
                        {
                            NewPupilLookup.Add(record);
                        }
                        else
                        {
                            var key = (string) record.laestab + (string) record.pupil_id;
                            if (!string.IsNullOrEmpty(key) && !ExistingPupilLookup.ContainsKey(key))
                            {
                                ExistingPupilLookup[key] = record;
                            }
                            else
                            {
                                log($"Could get key data for existing pupil amendment: {(string) record.amendment_id}");
                            }
                        }
                    }

                    log($"{amendmentCsvFilePath} loaded");
                }
            }
        }
    }
}
