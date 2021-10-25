using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaskLiner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }
    }
}
