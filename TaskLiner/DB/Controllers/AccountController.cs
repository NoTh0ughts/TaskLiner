using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using TaskLiner.DB.Controllers.Commands;
using TaskLiner.DB.Entity.Views;
using TaskLiner.DB.Controllers.Commands.User;

namespace TaskLiner.DB.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger,
                                 IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(string username, string password)
        {
            var newUser = await _mediator.Send(new PostRegisterUser());

            return null;
        }


        [HttpPost("/token")]
        [ProducesResponseType(typeof(UserWithToken), 400)]
        [ProducesResponseType(typeof(UserWithToken), 200)]
        public async Task<IActionResult> Token(string username, string password)
        {
            var user = await _mediator.Send(
                new GetUserTokenQuery() { Nickname = username, Password = password });

            if(user == null)
                return BadRequest();

            return Ok(user);
        }
        
        
        [HttpGet(nameof(IsCorrectUsernameAsync))]
        [ProducesResponseType(typeof(bool), 400)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> IsCorrectUsernameAsync([FromRoute] string username, CancellationToken ct)
        {
            var isCorrect = await _mediator.Send(new GetIsCorrectUserNameQuery() 
                                                { Nickname = username }, ct);
            if (isCorrect) return BadRequest();
            return Ok();
        }        
    }
}