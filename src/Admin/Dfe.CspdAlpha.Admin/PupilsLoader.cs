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

            using (var reader = new StreamReader(pupilsCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                log($"{pupilsCsvFilePath} loaded");

                var batchNumber = 1;

                foreach (IEnumerable<dynamic> batch in records.Batch(1000))
                {
                    Parallel.ForEach(batch, new ParallelOptions { MaxDegreeOfParallelism = MAX_PARALLELISM }, pupilRow =>
                    {
                        var gias = giasLookup[pupilRow.DFESNumber];
                        
                        pupilRow.URN = gias.urn;

                        var perf = (IEnumerable<dynamic>)performanceLookup[pupilRow.PortlandStudentID];

                        pupilRow.performance = perf.Select(r => new { r.Code, r.CodeValue });
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
        }
    }
}
