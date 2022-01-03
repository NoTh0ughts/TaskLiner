using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskLiner.DB.Controllers.Commands.Projects;
using TaskLiner.DB.Entity;

namespace TaskLiner.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Project), 200)]
        public async Task<IActionResult> Get(int id, int user_id)
        {
            var task = await _mediator.Send(new GetProjectQuery() { Id = id, User_id = user_id });

            if (task is null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("user/{user_id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Project), 200)]
        public async Task<IActionResult> GetProjectsForUser(int user_id)
        {
            var projects = await _mediator.Send(new GetProjectsForUserQuery() { UserId = user_id });

            if (projects is null)
                return NotFound();

            return Ok(projects);
        }

        [HttpPost(nameof(AddProject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Project), 200)]
        public async Task<IActionResult> AddProject(string name, string scope, string description, int company_id)
        {
            var project = await _mediator.Send(new PostProjectCommand()
            {
                Name = name,
                Scope = scope,
                Description = description,
                Company_Id = company_id
            });

            if (project is null)
                return BadRequest();
            return Ok(project);
        }
    }
}
