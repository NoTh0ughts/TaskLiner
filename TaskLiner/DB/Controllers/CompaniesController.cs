using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskLiner.DB.Controllers.Commands.Companys;
using TaskLiner.DB.Entity;

namespace TaskLiner.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(typeof(Company), 404)]
        public async Task<IActionResult> GetCompaniesForUser(int id)
        {
            var companies = await _mediator.Send(new GetCompaniesForUserQuery() { User_id = id });

            if (companies is null)
                return NotFound();

            return Ok(companies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(typeof(Company), 404)]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _mediator.Send(new GetCompanyQuery() { Company_id = id });
            if (company is null)
                return NotFound();

            return Ok(company);
        }

        [HttpGet("workers/{company_id}")]
        [ProducesResponseType(typeof(IEnumerator<User>), 200)]
        [ProducesResponseType(typeof(IEnumerator<User>), 404)]
        public async Task<IActionResult> GetWorkers(int company_id)
        {
            var workers = await _mediator.Send(new GetWorkersQuery() { Company_id = company_id });
            if (workers is null)
                return NotFound();

            return Ok(workers);
        }

        [HttpPost(nameof(AddWorker))]
        [ProducesResponseType(typeof(WorkerContract), 400)]
        [ProducesResponseType(typeof(WorkerContract), 201)]
        public async Task<IActionResult> AddWorker(int user_id, int company_id)
        {
            var contract = await _mediator.Send(new PostWorkerContractCommand()
            { Company_id = company_id, User_id = user_id });
            if (contract is null)
                return BadRequest();
            return Ok(contract);
        }


        [HttpPost(nameof(AddCompany))]
        [ProducesResponseType(typeof(Company), 400)]
        [ProducesResponseType(typeof(Company), 201)]
        
        public async Task<IActionResult> AddCompany(int user_id, string name, string description)
        {
            var company = await _mediator.Send(new PostCompanyCommand() 
                                                { Name = name, Description = description });
            
            if(company is not null)
            {
                var contract = await _mediator.Send(new PostWorkerContractCommand()
                { Company_id = company.Id, User_id = user_id, IsOwner = true });

                if (contract is null) return BadRequest();

                return Created($"/api/companys/{company.Id}", company);
            }
            return BadRequest();
        }   
    }
}
