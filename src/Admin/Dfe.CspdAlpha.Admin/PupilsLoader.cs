using CsvHelper;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.CspdAlpha.Admin
{
    public static class PupilsLoader
    {
#if DEBUG
        private const int MAX_PARALLELISM = 1;
#else
        private const int MAX_PARALLELISM = 10;
#endif

        private static volatile int _processedCount;

        public static void Load(
            Action<string> log, string pupilsCsvFilePath, string pupilsPerfCsvFilePath, string giasCsvFilePath, string amendmentsCsvFilePath = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            log($"{nameof(PupilsLoader)} started");

            ILookup<string, object> performanceLookup;
            var decimalConverter = new NumberJsonConverter();

            using (var reader = new StreamReader(pupilsPerfCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                performanceLookup = records.ToLookup(r => (string)r.PortlandStudentID);
                log($"{pupilsPerfCsvFilePath} loaded");
            }

            var giasLookup = new GiasLookup(log, giasCsvFilePath);
            var amendmentsLookup = new AmendmentLookup(log, amendmentsCsvFilePath);
            var skippedPupils = new List<string>();

            using (var reader = new StreamReader(pupilsCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                log($"{pupilsCsvFilePath} loaded");

                var batchNumber = 1;
                string[] resultsFieldsToRemove = new[] 
                    { 
                        "PortlandStudentID", 
                        "DFESNumber",
                        "CandidateNumber",
                        "SchoolCandidateNumber"
                    };

                foreach (IEnumerable<dynamic> batch in records.Batch(1000))
                {
                    Parallel.ForEach(batch, new ParallelOptions { MaxDegreeOfParallelism = MAX_PARALLELISM }, pupilRow =>
                    {
                        var laestab = (string)pupilRow.DFESNumber;
                        var gias = giasLookup[laestab];
                        if (gias == null)
                        {
                            skippedPupils.Add($"{pupilRow.CandidateNumber} ({pupilRow.DFESNumber})");
                            return;
                        }

                        var pupilID = string.Empty; // (string) pupilRow.UPN;
                        if (amendmentsLookup.ExistingPupilLookup.ContainsKey(laestab + pupilID))
                        {
                            // Handle existing pupil
                            var existingAmendment = amendmentsLookup.ExistingPupilLookup[laestab + pupilID];
                            var newURN = (string) existingAmendment.urn;
                            var newLAEstab = giasLookup.UrnLookup[newURN];
                            pupilRow.URN = newURN;
                            pupilRow.DFESNumber = newLAEstab;
                        }
                        else
                        {
                            pupilRow.URN = gias.urn;
                            var anonymisedName = ConvertToAlphaCharaters(pupilRow.CandidateNumber);
                            pupilRow.Surname = $"{anonymisedName}S";
                            pupilRow.Forename = $"{anonymisedName}F";
                        }

                        var perf = (IEnumerable<dynamic>)performanceLookup[pupilRow.PortlandStudentID];

                        pupilRow.performance = perf.Select(r =>
                        {
                            var dict = (IDictionary<string, object>) r;

                            resultsFieldsToRemove.Select(dict.Remove).ToList();

                            return dict;
                        });
                        System.Threading.Interlocked.Increment(ref _processedCount);
                    });

                    File.WriteAllText(
                        "pupils_batch_" + batchNumber + ".json",
                        JsonConvert.SerializeObject(batch, decimalConverter));

                    log($"{batchNumber} batches processed");
                    batchNumber++;
                }

                var addPupils = new List<dynamic>();
                foreach (var pupil in amendmentsLookup.NewPupilLookup)
                {
                    var urn = (string) pupil.urn;
                    dynamic addPupil = new ExpandoObject();
                    addPupil.DFESNumber = giasLookup.UrnLookup[urn];
                    addPupil.id = pupil.pupil_id;
                    addPupil.Forename = pupil.forename;
                    addPupil.Surname = pupil.surname;
                    addPupil.Gender = (string) pupil.gender == "Male" ? "M" : "F";
                    addPupil.DOB = ConvertDate((string) pupil.date_of_birth);
                    addPupil.ENTRYDAT = ConvertDate((string) pupil.admission_date);
                    addPupil.ActualYearGroup = pupil.year_group;
                    addPupil.performance = new string[0];
                    addPupil.URN = urn;
                    addPupils.Add(addPupil);
                }
                File.WriteAllText(
                    "pupils_batch_" + batchNumber + ".json",
                    JsonConvert.SerializeObject(addPupils, decimalConverter));
            }

            

            stopwatch.Stop();
            log($"{nameof(PupilsLoader)} finished in {stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds}s");

            if (skippedPupils.Count > 0)
            {
                log($"{skippedPupils.Count} skipped pupils (no GIAS establishment record found): {string.Join(", ", skippedPupils)}");
            }
        }

        private static string ConvertToAlphaCharaters(string candidateNumber)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new string(candidateNumber.Select(c => charDictionary[c]).ToArray()));
        }

        private static Dictionary<char, char> charDictionary => new Dictionary<char, char>
        {
            {'1', 'a' },
            {'2', 'b' },
            {'3', 'c' },
            {'4', 'd' },
            {'5', 'e' },
            {'6', 'f' },
            {'7', 'g' },
            {'8', 'h' },
            {'9', 'i' },
            {'0', 'j' }
        };

        private static string ConvertDate(string date)
        {
            if (DateTime.TryParse(date, out var convertedDate))
            {
                return convertedDate.ToString("yyyyMMdd");
            }

            return string.Empty;
        }
    }
}
