using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Models;
using Dfe.Rscd.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingwindow}/[controller]")]
    [ApiController]
    public class AmendmentsController : ControllerBase
    {
        private readonly IAmendmentService _amendmentService;

        public AmendmentsController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        // GET: api/Amendments/123456
        [HttpGet]
        [Route("id/{id}")]
        [SwaggerOperation(
            Summary = "Returns the requested amendment",
            Description = "Returns the amendment specified by the id",
            OperationId = "GetAmendment",
            Tags = new[] {"Amendments"}
        )]
        [ProducesResponseType(typeof(GetResponse<AmendmentDTO>), 200)]
        public IActionResult GetAmendment(
            [FromRoute] [SwaggerParameter("The id of the requested amendment", Required = true)]
            string id,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            string checkingwindow)
        {
            var amendment = _amendmentService.GetAmendment(checkingwindow.ToDomainCheckingWindow(), id);

            var response = new GetResponse<IAmendment>
            {
                Result = amendment,
                Error = new Error()
            };
            return Ok(response);
        }

        // GET: api/Amendments/123456
        [HttpGet]
        [Route("urn/{urn}")]
        [SwaggerOperation(
            Summary = "Searches for amendments by school or college",
            Description = "Searches for requested amendments in CRM recorded against the supplied URN.",
            OperationId = "GetAmendments",
            Tags = new[] {"Amendments"}
        )]
        [ProducesResponseType(typeof(GetResponse<List<IAmendment>>), 200)]
        public IActionResult GetAmendments(
            [FromRoute] [SwaggerParameter("The URN of the school requesting amendments", Required = true)]
            string urn,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            string checkingwindow)
        {
            var amendments = _amendmentService
                .GetAmendments(checkingwindow.ToDomainCheckingWindow(), urn)
                .ToList();

            var response = new GetResponse<List<IAmendment>>
            {
                Result = amendments,
                Error = new Error()
            };

            return Ok(response);
        }


        // POST: api/Amendments
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates an add pupil amendment in CRM",
            Description = "Creates an amendment linked to an establishment and checking phase in CRM",
            OperationId = "CreateAmendment",
            Tags = new[] {"Amendments"}
        )]
        [ProducesResponseType(typeof(GetResponse<string>), 200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] [SwaggerRequestBody("Amendment to add to CRM", Required = true)] AddPupilAmendment amendment)
        {
            try
            {
                var amendmentReference = _amendmentService.CreateAmendment(amendment);
                var response = new GetResponse<string>
                {
                    Result = amendmentReference,
                    Error = new Error()
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                var response = new GetResponse<string>
                {
                    Result = string.Empty,
                    Error = new Error
                    {
                        ErrorMessage = e.Message
                    }
                };
                return BadRequest(response);
            }
        }

        // POST: api/Amendments
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a remove pupil amendment in CRM",
            Description = "Creates an amendment linked to an establishment and checking phase in CRM",
            OperationId = "CreateAmendment",
            Tags = new[] { "Amendments" }
        )]
        [ProducesResponseType(typeof(GetResponse<string>), 200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody][SwaggerRequestBody("Amendment to add to CRM", Required = true)] RemovePupilAmendment amendment)
        {
            try
            {
                var amendmentReference = _amendmentService.CreateAmendment(amendment);
                var response = new GetResponse<string>
                {
                    Result = amendmentReference,
                    Error = new Error()
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                var response = new GetResponse<string>
                {
                    Result = string.Empty,
                    Error = new Error
                    {
                        ErrorMessage = e.Message
                    }
                };
                return BadRequest(response);
            }
        }

        [HttpPut]
        [Route("relateevidence")]
        [SwaggerOperation(
            Summary = "Relates evidence to an amendment",
            Description = "Creates an entity relationship between an amendment and evidence documents",
            OperationId = "RelateEvidence",
            Tags = new[] {"Amendments", "Evidence"}
        )]
        [ProducesResponseType(typeof(GetResponse<bool>), 200)]
        public IActionResult RelateEvidence([FromBody] [SwaggerRequestBody("Amendment to add to CRM", Required = true)]
            RelateEvidenceDTO relateEvidenceDto)
        {
            _amendmentService.RelateEvidence(relateEvidenceDto.AmendmentID, relateEvidenceDto.EvidenceFolderName, true);

            var response = new GetResponse<bool>
            {
                Result = true,
                Error = new Error()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Cancel an amendment",
            Description = "Cancels the amendment related to the provided id",
            OperationId = "CancelAmendment",
            Tags = new[] {"Amendments"})]
        [ProducesResponseType(typeof(GetResponse<bool>), 200)]
        public IActionResult Delete([FromRoute] [SwaggerParameter("The id of the amendment to cancel", Required = true)]
            string id)
        {
            var result = _amendmentService.CancelAmendment(id);
            var response = new GetResponse<bool>
            {
                Result = result,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("/api/[controller]")]
        [Consumes("text/csv")]
        [Produces("text/csv")]
        [SwaggerOperation(
            Summary = "Generates CSV file of all recorded accepted amendments",
            Description = "Generates CSV file of all recorded accepted amendments.",
            OperationId = "DownloadAmendmentsCsv",
            Tags = new[] {"Amendments"}
        )]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Get()
        {
            // TODO: This is a temporary download endpoint and not considered the final solution
            // for exporting amendments to the data pipeline (or any other consumers).
            // This is also likely to become an expensive operation, so caching will be important.
            var amendments = _amendmentService.GetAmendments();

            var stream = new MemoryStream();
            using (TextWriter writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                var csv = new CsvWriter(writeFile, CultureInfo.InvariantCulture);

                foreach (var amendment in amendments)
                {
                    csv.WriteRecord(amendment);
                    csv.NextRecord();
                }
            }

            stream.Position = 0;

            return File(
                stream, "text/csv", $"Amendments-{DateTime.UtcNow:yyyyMMddTHHmmss}.csv");
        }
    }
}