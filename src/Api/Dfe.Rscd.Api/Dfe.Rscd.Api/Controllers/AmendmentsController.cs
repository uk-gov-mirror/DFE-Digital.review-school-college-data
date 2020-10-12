using System;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Models;
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

        // GET: api/Amendments/123456
        [HttpGet]
        [Route("getamendments/{urn}")]
        [SwaggerOperation(
            Summary = "Searches for schools requested amendments",
            Description = "Searches for requested amendments in CRM recorded against the supplied URN.",
            OperationId = "GetAmendments",
            Tags = new[] { "Amendments" }
        )]
        [ProducesResponseType(typeof(GetResponse<List<Amendment>>), 200)]
        public IActionResult GetAmendments([FromRoute, SwaggerParameter("The URN of the school requesting amendments", Required = true)] string urn,
            [FromRoute, SwaggerParameter("The checking window to request amendments from", Required = true)] string checkingwindow)
        {
            CheckingWindow checkingWindow;
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out checkingWindow);
            var lateChecking = checkingwindow == "ks4-late";

            var amendments = _amendmentService.GetAmendments(checkingWindow, urn)
                .ToList();
            var response = new GetResponse<List<Amendment>>
            {
                Result = amendments,
                Error = new Error()
            };
            return Ok(response);
        }



        // GET: api/Amendments/123456
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
            CheckingWindow checkingWindow;
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out checkingWindow);
            var lateChecking = checkingwindow == "ks4-late";

            var amendments = _amendmentService.GetAddPupilAmendments(checkingWindow, urn)
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
        [SwaggerOperation(
            Summary = "Creates an amendment in CRM",
            Description = "Creates an amendment linked to an establishment and checking phase in CRM",
            OperationId = "CreateAmendment",
            Tags = new[] { "Amendments" }
        )]
        [ProducesResponseType(typeof(GetResponse<string>), 200)]
        public IActionResult Post([FromBody, SwaggerRequestBody("Amendment to add to CRM", Required = true)] AmendmentDTO amendmentDto)
        {
            var amendment = amendmentDto.Amendment;
            amendment.AmendmentDetail = amendmentDto.RemovePupil;
            var amendmentReference = _amendmentService.CreateAmendment(amendment);
            var response = new GetResponse<string>
            {
                Result = amendmentReference,
                Error = new Error()
            };
            return Ok(response);
        }




        // PUT: api/Amendments/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
