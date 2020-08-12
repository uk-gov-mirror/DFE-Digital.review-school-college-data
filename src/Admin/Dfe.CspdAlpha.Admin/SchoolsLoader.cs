using CsvHelper;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            var decimalConverter = new NumberJsonConverter();

            using (var reader = new StreamReader(schoolsPerfCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                performanceLookup = records.ToLookup(r => (string)r.DFESNumber);
                log($"{schoolsPerfCsvFilePath} loaded");
            }

            var giasLookup = new GiasLookup(log, giasCsvFilePath);

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
                        var gias = giasLookup[schoolRow.DFESNumber];
                        if (gias != null)
                        {
                            schoolRow.URN = gias.urn;
                            schoolRow.SchoolType = gias.typeofestablishmentname;
                            schoolRow.SchoolName = $"Test School {gias.urn}";
                            var perf = (IEnumerable<dynamic>) performanceLookup[schoolRow.DFESNumber];
                            schoolRow.performance = perf.Select(r => new {r.Code, r.SetName, r.CodeValue});
                            System.Threading.Interlocked.Increment(ref _processedCount);
                        }
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
