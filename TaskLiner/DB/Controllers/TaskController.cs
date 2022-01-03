using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskLiner.DB.Controllers.Commands.Task;
using TaskEntity = TaskLiner.DB.Entity.Task;

namespace TaskLiner.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(TaskEntity), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _mediator.Send(new GetTaskQuery() { Id = id });

            if (task is null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost(nameof(AddTask))]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(TaskEntity), 200)]
        public async Task<IActionResult> AddTask(PostTaskCommand resource)
        {
            var newTask = await _mediator.Send(resource);

            if (newTask is null)
                return BadRequest();

            return Ok(newTask);
        }
    }
}
