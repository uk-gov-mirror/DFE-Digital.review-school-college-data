using System;
using System.Threading.Tasks;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Models;
using Dfe.Rscd.Api.Models.SearchRequests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingwindow}/[controller]")]
    [ApiController]
    public class EstablishmentsController : ControllerBase
    {
        private readonly IEstablishmentService _establishmentService;

        public EstablishmentsController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        [HttpGet]
        [Route("{urn}")]
        [SwaggerOperation(
            Summary = "Searches for a school",
            Description = "Searches for a school identified by the supplied URN and returns an Establishment object.",
            OperationId = "GetEstablishmentByURN",
            Tags = new[] {"Establishments"}
        )]
        [ProducesResponseType(typeof(GetResponse<Establishment>), 200)]
        public IActionResult Get(
            [FromRoute] [SwaggerParameter("The URN of the school requesting amendments", Required = true)]
            string urn,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            string checkingwindow)
        {
            var urnValue = new URN(urn);
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out CheckingWindow checkingWindow);
            var establishmentData = _establishmentService.GetByURN(checkingWindow, urnValue);
            var response = new GetResponse<Establishment>
            {
                Result = establishmentData,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        [SwaggerOperation(
            Summary = "Searches for schools or colleges.",
            Description = @"Searches for schools or colleges based on the supplied query parameters.",
            OperationId = "SearchTEstablishments",
            Tags = new[] {"Establishments"}
        )]
        [ProducesResponseType(typeof(GetResponse<Establishment>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Search(
            [FromQuery] [SwaggerParameter("Event search criteria.", Required = true)]
            EstablishmentsSearchRequest request,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            string checkingwindow)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            CheckingWindow checkingWindow;
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out checkingWindow);
            var establishmentData = _establishmentService.GetByDFESNumber(checkingWindow, request.DFESNumber);
            var response = new GetResponse<Establishment>
            {
                Result = establishmentData,
                Error = new Error()
            };
            return Ok(response);
        }
    }
}