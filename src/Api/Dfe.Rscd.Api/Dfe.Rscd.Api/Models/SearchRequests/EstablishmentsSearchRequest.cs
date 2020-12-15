using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Models.SearchRequests
{
    public class EstablishmentsSearchRequest
    {
        [SwaggerSchema("DfesNumber to filter establishments on")]
        public string DFESNumber { get; set; }
    }
}