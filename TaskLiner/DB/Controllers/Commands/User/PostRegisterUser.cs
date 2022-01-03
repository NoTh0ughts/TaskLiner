using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TaskLiner.DB.Entity.Views;

namespace TaskLiner.DB.Controllers.Commands.User
{
    public class PostRegisterUser : IRequest<UserPublicResource>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
