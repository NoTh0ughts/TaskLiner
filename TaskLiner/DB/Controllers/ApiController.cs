using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskLiner.DB.Entity;
using TaskLiner.Service.Attributes;

namespace TaskLiner.DB.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetTestValue")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> GetTestValue(CancellationToken ct)
        {
            try
            {
                await using var db = new TaskLinerContext();
                return Ok("Result: " + db.Companies + " Count : " + db.Companies.Count());
            }
            catch (Exception e)
            {
                return BadRequest("Error: "+ e);
            }
        }
    }
}
