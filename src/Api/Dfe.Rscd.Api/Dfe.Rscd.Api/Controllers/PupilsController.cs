using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingwindow}/[controller]")]
    [ApiController]
    public class PupilsController : ControllerBase
    {
        private IPupilService _pupilService;

        public PupilsController(IPupilService pupilService)
        {
            _pupilService = pupilService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Searches for a pupil",
            Description = "Searches for a pupil identified by the supplied id and returns an Pupil object.",
            OperationId = "GetPupilById",
            Tags = new[] {"Pupils"}
        )]
        [ProducesResponseType(typeof(GetResponse<Pupil>), 200)]
        public IActionResult Get(
            [FromRoute, SwaggerParameter("The id of the pupil requesting amendments", Required = true)]
            string id,
            [FromRoute, SwaggerParameter("The checking window to request pupil from", Required = true)]
            string checkingwindow)
        {
            CheckingWindow checkingWindow;
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out checkingWindow);
            var pupil = _pupilService.GetById(checkingWindow, id);
            var response = new GetResponse<Pupil>
            {
                Result = pupil,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        [SwaggerOperation(
            Summary = "Searches for a pupil or pupils.",
            Description = @"Searches for pupils based on the supplied query parameters.",
            OperationId = "SearchPupils",
            Tags = new[] { "Pupils" }
            )]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<Pupil>>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Search([FromQuery, SwaggerParameter("Pupil search criteria.", Required = true)] PupilsSearchRequest request,
        [FromRoute, SwaggerParameter("The checking window to request pupil or pupils from", Required = true)] string checkingwindow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            CheckingWindow checkingWindow;
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out checkingWindow);
            var pupils = _pupilService.QueryPupils(checkingWindow, request);
            var response = new GetResponse<IEnumerable<Pupil>>
            {
                Result = pupils,
                Error = new Error()
            };
            return Ok(response);
        }

    }
}