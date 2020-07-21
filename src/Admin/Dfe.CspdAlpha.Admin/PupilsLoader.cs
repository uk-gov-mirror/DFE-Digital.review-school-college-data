using CsvHelper;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Action<string> log, string pupilsCsvFilePath, string pupilsPerfCsvFilePath, string giasCsvFilePath)
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
                        var gias = giasLookup[pupilRow.DFESNumber];

                        if (gias == null)
                        {
                            skippedPupils.Add($"{pupilRow.CandidateNumber} ({pupilRow.DFESNumber})");

                            return;
                        }
                        
                        pupilRow.URN = gias.urn;
                        pupilRow.Surname = $"{pupilRow.CandidateNumber}S";
                        pupilRow.Forename = $"{pupilRow.CandidateNumber}F";

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
            }

            stopwatch.Stop();
            log($"{nameof(PupilsLoader)} finished in {stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds}s");

            if (skippedPupils.Count > 0)
            {
                log($"{skippedPupils.Count} skipped pupils (no GIAS establishment record found): {string.Join(", ", skippedPupils)}");
            }
        }
    }
}
