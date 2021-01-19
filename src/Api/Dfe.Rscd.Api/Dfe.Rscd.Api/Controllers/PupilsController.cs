using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Models;
using Dfe.Rscd.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingwindow}/[controller]")]
    [ApiController]
    public class PupilsController : ControllerBase
    {
        private readonly IPupilService _pupilService;
        private readonly IPromptService _promptService;

        public PupilsController(IPupilService pupilService, IPromptService promptService)
        {
            _pupilService = pupilService;
            _promptService = promptService;
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
            [FromRoute] [SwaggerParameter("The id of the pupil requesting amendments", Required = true)]
            string id,
            [FromRoute] [SwaggerParameter("The checking window to request pupil from", Required = true)]
            string checkingwindow)
        {
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out CheckingWindow checkingWindow);
            var pupil = _pupilService.GetById(checkingWindow, id);
            var response = new GetResponse<Pupil>
            {
                Result = pupil,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/remove-prompts/{reasonId}")]
        [SwaggerOperation(
            Summary = "Returns the requested amendment prompt questions",
            Description = "Returns the requested amendment prompt questions by adjustment reason id and pupil id",
            OperationId = "GetRemoveAmendmentPrompts",
            Tags = new[] {"Pupils"}
        )]
        [ProducesResponseType(typeof(GetResponse<AdjustmentOutcome>), 200)]
        public IActionResult GetAmendPrompts(
            [FromRoute] [SwaggerParameter("The id of the student/pupil", Required = true)]
            string id,
            [FromRoute] [SwaggerParameter("The id of the adjustment reason", Required = true)]
            int reasonId,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            string checkingwindow)
        {
            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out CheckingWindow checkingWindow);

            var pupil = _pupilService.GetById(checkingWindow, id);

            var prompts = _promptService.GetAdjustmentPrompts(checkingWindow, pupil.DfesNumber, pupil.Id, reasonId);

            var response = new GetResponse<AdjustmentOutcome>
            {
                Result = prompts,
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
            Tags = new[] {"Pupils"}
        )]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<PupilRecord>>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Search(
            [FromQuery] [SwaggerParameter("Pupil search criteria.", Required = true)]
            PupilsSearchRequest request,
            [FromRoute] [SwaggerParameter("The checking window to request pupil or pupils from", Required = true)]
            string checkingwindow)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Enum.TryParse(checkingwindow.Replace("-", string.Empty), true,
                out CheckingWindow checkingWindow);
            var pupils = _pupilService.QueryPupils(checkingWindow, request);
            var response = new GetResponse<IEnumerable<PupilRecord>>
            {
                Result = pupils,
                Error = new Error()
            };
            return Ok(response);
        }
    }
}