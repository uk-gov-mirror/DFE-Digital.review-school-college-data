using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingwindow}/[controller]")]
    [ApiController]
    public class AmendmentsController : ControllerBase
    {
        private IAmendmentService _amendmentService;

        public AmendmentsController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        // TODO: Not sure if we need a get all amendments endpoint as we will likely have a dedicated end point for the amendments export
        // GET: api/Amendments
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{

        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Amendments/5
        [HttpGet]
        [Route("{urn}")]
        [SwaggerOperation(
            Summary = "Searches for schools requested amendments",
            Description = "Searches for requested amendments in CRM recorded against the supplied URN.",
            OperationId = "GetAmendmentsByURN",
            Tags = new[] {"Amendments"}
            )]
        [ProducesResponseType(typeof(GetResponse<List<AddPupilAmendment>>), 200)]
        public IActionResult Get([FromRoute, SwaggerParameter("The URN of the school requesting amendments", Required = true)] string urn,
            [FromRoute, SwaggerParameter("The checking window to request amendments from", Required = true)] string checkingwindow)
        {
            var lateChecking = checkingwindow == "ks4-late";
            var amendments = _amendmentService.GetAddPupilAmendments(urn)
                .Where(a => !lateChecking || (a.Status == "Approved" || a.Status == "Rejected"))
                .OrderByDescending(o => o.CreatedDate)
                .ToList();
            var response = new GetResponse<List<AddPupilAmendment>>
            {
                Result = amendments,
                Error = new Error()
            };
            return Ok(response);
        }

        // POST: api/Amendments
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Amendments/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
