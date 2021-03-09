using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Net.Http;

namespace Dfe.Rscd.Api
{
    /// <summary>
    /// Custom Application Insights telemetry initializer for capturing CRM request messages
    /// </summary>
    public class CrmTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var dependency = telemetry as DependencyTelemetry;
            if (dependency == null) return;

            object requestObj;
            bool found = dependency.TryGetOperationDetail("HttpRequest", out requestObj);

            if (!found)
            {
                return;
            }

            var request = requestObj as HttpRequestMessage;

            // Searching for this header seems to be the most obvious available means
            // to identify CRM requests.
            if (request == null || !request.Headers.Contains("SOAPAction"))
            {
                return;
            }

            const string propKey = "Request message";

            if (!dependency.Properties.ContainsKey(propKey))
            {
                dependency.Properties.Add(propKey, request.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
