﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Models;
using Dfe.Rscd.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IDataService _commonDataService;

        public ReferenceDataController(IDataService commonDataService)
        {
            _commonDataService = commonDataService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Amend codes",
            Description = @"Displays a lists of a common reference data list Amend code",
            OperationId = "AmendCodes",
            Tags = new[] {"CommonLists"}
        )]
        [Route("/amendcodes")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<AmendCode>>), 200)]
        public async Task<IActionResult> GetAmendReference()
        {
            var list = _commonDataService.GetAmendCodes();
            var response = new GetResponse<IEnumerable<AmendCode>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Awarding bodies",
            Description = @"Displays a lists of a common reference data list Awarding bodies",
            OperationId = "AwardingBodies",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/awardingbodies")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<AwardingBody>>), 200)]
        public async Task<IActionResult> GetAwardingBodies()
        {
            var list = _commonDataService.GetAwardingBodies();
            var response = new GetResponse<IEnumerable<AwardingBody>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Ethnicities",
            Description = @"Displays a lists of a common reference data list Ethnicities",
            OperationId = "Ethnicities",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/ethnicities")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<Ethnicity>>), 200)]
        public async Task<IActionResult> GetEthnicities()
        {
            var list = _commonDataService.GetEthnicities();
            var response = new GetResponse<IEnumerable<Ethnicity>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Inclusion adjustment reasons",
            Description = @"Displays a lists of a common reference data list Inclusion adjustment reasons",
            OperationId = "InclusionAdjustmentReasons",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/inclusion-adjustment-reasons")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<InclusionAdjustmentReason>>), 200)]
        public async Task<IActionResult> GetInclusionAdjustmentReasons()
        {
            var list = _commonDataService.GetInclusionAdjustmentReasons();
            var response = new GetResponse<IEnumerable<InclusionAdjustmentReason>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Inclusion adjustment reasons allowed for a certain pupil inclusion number",
            Description = @"Displays a lists of a common reference data list Inclusion adjustment reasons",
            OperationId = "InclusionAdjustmentReasons",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/inclusion-adjustment-reasons/pincl/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<InclusionAdjustmentReason>>), 200)]
        public async Task<IActionResult> GetInclusionAdjustmentReasonsByPincl(
            [FromRoute] [SwaggerParameter("The pupil inclusion identifier", Required = true)]
            string id)
        {
            var list = _commonDataService.GetInclusionAdjustmentReasons(id);
            var response = new GetResponse<IEnumerable<InclusionAdjustmentReason>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of languages",
            Description = @"Displays a lists of a common reference data list languages",
            OperationId = "Languages",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/languages")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<FirstLanguage>>), 200)]
        public async Task<IActionResult> GetLanguages()
        {
            var list = _commonDataService.GetLanguages();
            var response = new GetResponse<IEnumerable<FirstLanguage>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Pincludes",
            Description = @"Displays a lists of a common reference data list PIncludes",
            OperationId = "PINCLs",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/pincludes")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<PINCLs>>), 200)]
        public async Task<IActionResult> GetPINCls()
        {
            var list = _commonDataService.GetPINCLs();
            var response = new GetResponse<IEnumerable<PINCLs>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of Sen statuses",
            Description = @"Displays a lists of a common reference data list Sen statuses",
            OperationId = "SenStatus",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/senstatuses")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<SENStatus>>), 200)]
        public async Task<IActionResult> GetSenStatuses()
        {
            var list = _commonDataService.GetSENStatus();
            var response = new GetResponse<IEnumerable<SENStatus>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Display a list of year groups",
            Description = @"Displays a lists of a common reference data list year groups",
            OperationId = "YearGroup",
            Tags = new[] { "CommonLists" }
        )]
        [Route("/yeargroups")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(GetResponse<IEnumerable<YearGroup>>), 200)]
        public async Task<IActionResult> GetYearGroups()
        {
            var list = _commonDataService.GetYearGroups();
            var response = new GetResponse<IEnumerable<YearGroup>>
            {
                Result = list,
                Error = new Error()
            };
            return Ok(response);
        }
    }
}