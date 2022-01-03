using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands
{
    public class GetIsCorrectUserNameQuery
        : IRequest<bool>
    {
        public string Nickname { get; set; }

        public class GetIsCorrectUserNameQueryHandler : IRequestHandler<GetIsCorrectUserNameQuery, bool>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetIsCorrectUserNameQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(GetIsCorrectUserNameQuery request, CancellationToken cancellationToken)
            {
                 var isExists = await _unitOfWork.DbContext.Users
                    .AnyAsync(x => x.Nickname == request.Nickname, cancellationToken);
                return isExists == true;
            }
        }
    }
}
