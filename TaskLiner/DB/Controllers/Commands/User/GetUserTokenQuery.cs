using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskLiner.DB.Entity;
using UserEntity = TaskLiner.DB.Entity.User;
using TaskLiner.DB.Entity.Views;
using TaskLiner.DB.UnitOfWork;
using TaskLiner.Service;

namespace TaskLiner.DB.Controllers.Commands
{
    public class GetUserTokenQuery : IRequest<UserWithToken>
    {
        public string Nickname { get; set; }
        public string Password { get; set; }


        public class GetUserTokenQueryHandler : IRequestHandler<GetUserTokenQuery, UserWithToken>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetUserTokenQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserWithToken> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
            {
                var (identity, user) = await GetIdentity(request.Nickname, request.Password);
                if (identity == null)
                {
                    return null;
                }

                var nowdate = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.IDUSER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: nowdate,
                    claims: identity.Claims,
                    expires: nowdate.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new UserWithToken(user, encodedJwt);
            }

            private async Task<(ClaimsIdentity, UserEntity)> GetIdentity(string username, string password)
            {
                UserEntity user = await _unitOfWork.DbContext.Users.FirstOrDefaultAsync(x =>
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

                    return (claimsIdentity, user);
                }

                return (null, null);
            }
        }        
    }
}
