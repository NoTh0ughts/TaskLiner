using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Entity.Views;
using TaskLiner.DB.UnitOfWork;
using TaskLiner.Service;

namespace TaskLiner.DB.Controllers
{
    [ApiController] 
    [Route("/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork<TaskLinerContext> _taskLinerUnit;
        
        public AccountController(ILogger<AccountController> logger, 
            IUnitOfWork<TaskLinerContext> taskLinerUnit)
        {
            _logger = logger;
            _taskLinerUnit = taskLinerUnit;
        }


        [HttpPost("/token")]
        public async Task<IActionResult> Token(string username, string password)
        {
            var identity = await GetIdentity(username, password);
            if (identity == null)
                return BadRequest(new {errorText = "Invalid username or password"});

            var nowdate = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer:    AuthOptions.IDUSER,
                audience:  AuthOptions.AUDIENCE,
                notBefore: nowdate,
                claims:    identity.Claims,
                expires:   nowdate.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return new JsonResult(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            User user = await _taskLinerUnit.DbContext.Users.FirstOrDefaultAsync(x =>
                x.Nickname == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Nickname),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Proffesion)
                };
                
                var claimsIdentity = 
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
        
        
        [HttpGet(nameof(IsCorrectUsernameAsync))]
        public async Task<ActionResult> IsCorrectUsernameAsync([FromRoute] string username, CancellationToken ct)
        {
            var isExists = await IsCorrectUsernameAsyncPrivate(username, ct);
            if (isExists) return BadRequest("cant");
            return Ok("Correct");
        }

        
        private async Task<bool> IsCorrectUsernameAsyncPrivate(string username, CancellationToken ct)
        {
            var isExists = await _taskLinerUnit.DbContext.Users.AnyAsync(x => x.Nickname == username, ct);
            return isExists == true;
        }
    }
}