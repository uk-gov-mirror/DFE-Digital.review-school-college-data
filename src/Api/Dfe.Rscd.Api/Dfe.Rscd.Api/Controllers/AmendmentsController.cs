using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        // GET: api/Amendments
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET: api/Amendments/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(string id, string checkingwindow)
        {
            var lateChecking = checkingwindow == "ks4-late";
            var amendments = _amendmentService.GetAddPupilAmendments(id)
                .Where(a => !lateChecking || (a.Status == "Approved" || a.Status == "Rejected"))
                .OrderByDescending(o => o.CreatedDate)
                .ToList();
            var response = new GetResponse<List<AddPupilAmendment>>
            {
                Result = amendments
            };
            return JsonConvert.SerializeObject(response);
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
