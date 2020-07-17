using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Dfe.CspdAlpha.Admin
{
    public class GiasLookup
    {
        private IDictionary<string, dynamic> _lookup;
        private Action<string> _log;

        public dynamic this[string i]
        {
            get 
            {
                if (!_lookup.TryGetValue(i, out var giasRecord))
                {
                    _log($"can't find GIAS record for {i}");

                    return null;
                }
                
                var urn = giasRecord.urn;

                if (string.IsNullOrEmpty(urn))
                {
                    _log($"can't find URN for {i}");

                    return null;
                }

                return giasRecord; 
            }
        }

        public GiasLookup(Action<string> log, string giasCsvFilePath)
        {
            _log = log;

            using (var reader = new StreamReader(giasCsvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // TODO: Regex replace would probably be better here
                csv.Configuration.PrepareHeaderForMatch = (s, i) =>
                    s.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).ToLower().Trim();
                var records = csv.GetRecords<dynamic>();

                // TODO: we need to build the LAEstab/DFESNumber in GIAS however this can lead to duplicates possibly due to conversions
                // This code assumes the last match is the valid one and overwrites any existing values
                _lookup = new Dictionary<string, dynamic>();

                foreach (var record in records)
                {
                    var lacode = (string)record.lacode;
                    var establshmentnumber = (string)record.establishmentnumber;

                    if (string.IsNullOrEmpty(lacode) || string.IsNullOrEmpty(establshmentnumber))
                    {
                        continue;
                    }

                    var key = lacode + establshmentnumber;

                    if (_lookup.ContainsKey(key))
                    {
                        _lookup[key] = record;
                    }
                    else
                    {
                        _lookup.Add(key, record);
                    }
                }

                log($"{giasCsvFilePath} loaded");
            }
        }
    }
}
