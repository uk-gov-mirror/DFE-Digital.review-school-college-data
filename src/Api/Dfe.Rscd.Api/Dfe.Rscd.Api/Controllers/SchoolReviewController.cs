using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingwindow}/[controller]")]
    [ApiController]
    public class SchoolReviewController : ControllerBase
    {
        private IConfirmationService _confirmationService;

        public SchoolReviewController(IConfirmationService confirmationService)
        {
            _confirmationService = confirmationService;
        }

        [HttpGet]
        [Route("{userid}/{urn}")]
        [SwaggerOperation(
            Summary = "Returns the requested school review record",
            Description = "Returns the requested school review record specified by the supplied user id and urn",
            OperationId = "GetSchoolReviewRecord",
            Tags = new[] { "School review record" }
        )]
        [ProducesResponseType(typeof(GetResponse<ConfirmationRecord>), 200)]
        public IActionResult Get([FromRoute, SwaggerParameter("The id of the user requesting the school review record", Required = true)] string userid,
            [FromRoute, SwaggerParameter("The urn of the school requesting the school review record", Required = true)] string urn)
        {
            var schoolReviewRecord = _confirmationService.GetConfirmationRecord(userid, urn);
            var response = new GetResponse<ConfirmationRecord>
            {
                Result = schoolReviewRecord,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates the requested school review record",
            Description = "Creates the requested school review record specified by the supplied user id and urn",
            OperationId = "CreateSchoolReviewRecord",
            Tags = new[] { "School review record" }
        )]
        [ProducesResponseType(typeof(GetResponse<bool>), 200)]
        public IActionResult Post([FromBody, SwaggerParameter("The confirmation record to create", Required = true)] ConfirmationRecord record)
        {
            var schoolReviewCreated = _confirmationService.CreateConfirmationRecord(record);
            var response = new GetResponse<bool>
            {
                Result = schoolReviewCreated,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpPut]
        [SwaggerOperation(
            Summary = "Updates the requested school review record",
            Description = "Updates the requested school review record specified by the supplied user id and urn",
            OperationId = "UpdateSchoolReviewRecord",
            Tags = new[] { "School review record" }
        )]
        [ProducesResponseType(typeof(GetResponse<bool>), 200)]
        public IActionResult Put([FromBody, SwaggerParameter("The confirmation record to create", Required = true)] ConfirmationRecord record)
        {
            var schoolReviewUpdated = _confirmationService.UpdateConfirmationRecord(record);
            var response = new GetResponse<bool>
            {
                Result = schoolReviewUpdated,
                Error = new Error()
            };
            return Ok(response);
        }

    }
}