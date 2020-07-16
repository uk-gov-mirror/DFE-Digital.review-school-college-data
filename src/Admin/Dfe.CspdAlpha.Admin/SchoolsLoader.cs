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
    public static class SchoolsLoader
    {
#if DEBUG
        private const int MAX_PARALLELISM = 1;
#else
        private const int MAX_PARALLELISM = 10;
#endif

        private static volatile int _processedCount;

        public static void Load(Action<string> log, string schoolsRefCsvFilePath, string schoolsPerfCsvFilePath, string giasCsvFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            log($"{nameof(SchoolsLoader)} started");

            ILookup<string, object> performanceLookup;
            IDictionary<string, dynamic> giasDictionary;
            var decimalConverter = new NumberJsonConverter();

            using (var reader = new StreamReader(schoolsPerfCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                performanceLookup = records.ToLookup(r => (string)r.DFESNumber);
                log($"{schoolsPerfCsvFilePath} loaded");
            }


            using (var reader = new StreamReader(giasCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // TODO: Regex replace would probably be better here
                csv.Configuration.PrepareHeaderForMatch = (s, i) =>
                    s.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).ToLower().Trim();
                var records = csv.GetRecords<dynamic>();

                // TODO: we need to build the LAEstab/DFESNumber in GIAS however this can lead to duplicates possibly due to conversions
                // This code assumes the last match is the valid one and overwrites any existing values
                giasDictionary = new Dictionary<string, dynamic>();
                foreach (var record in records)
                {
                    var lacode = (string)record.lacode;
                    var establshmentnumber = (string)record.establishmentnumber;
                    if (string.IsNullOrEmpty(lacode) || string.IsNullOrEmpty(establshmentnumber))
                    {
                        continue;
                    }

                    var key = lacode + establshmentnumber;
                    if (giasDictionary.ContainsKey(key))
                    {
                        giasDictionary[key] = record;
                    }
                    else
                    {
                        giasDictionary.Add(key, record);
                    }
                }
                log($"{giasCsvFilePath} loaded");
            }

            using (var reader = new StreamReader(schoolsRefCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                log($"{schoolsRefCsvFilePath} loaded");

                var batchNumber = 1;

                foreach (IEnumerable<dynamic> batch in records.Batch(1000))
                {
                    Parallel.ForEach(batch, new ParallelOptions { MaxDegreeOfParallelism = MAX_PARALLELISM }, schoolRow =>
                    {
                        var gias = giasDictionary[schoolRow.DFESNumber];
                        var urn = gias.urn;
                        if (string.IsNullOrEmpty(urn))
                        {
                            log($"can't find URN for {schoolRow.DFESNumber}");
                        }
                        schoolRow.URN = urn;
                        schoolRow.SchoolType = gias.typeofestablishmentname;
                        schoolRow.SchoolName = $"Test School {urn}";
                        var perf = (IEnumerable<dynamic>)performanceLookup[schoolRow.DFESNumber];
                        schoolRow.performance = perf.Select(r => new { r.Code, r.SetName, r.CodeValue });
                        System.Threading.Interlocked.Increment(ref _processedCount);
                    });

                    File.WriteAllText(
                        "schools_batch_" + batchNumber + ".json",
                        JsonConvert.SerializeObject(batch, decimalConverter));

                    log($"{batchNumber} batches processed");
                    batchNumber++;
                }
            }

            stopwatch.Stop();
            log($"{nameof(SchoolsLoader)} finished in {stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds}s");
        }
    }
}
