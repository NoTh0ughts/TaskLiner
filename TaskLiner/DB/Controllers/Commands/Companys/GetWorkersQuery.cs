using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.UnitOfWork;
using TaskLiner.DB.Entity.Views;
using System.Linq.Expressions;

namespace TaskLiner.DB.Controllers.Commands.Companys
{
    public class GetWorkersQuery : IRequest<IEnumerable<UserPublicResource>>
    {
        public int Company_id { get; set; }

        public class GetWorkersQueryHandler : IRequestHandler<GetWorkersQuery, IEnumerable<UserPublicResource>>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetWorkersQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<UserPublicResource>> Handle(GetWorkersQuery request, CancellationToken cancellationToken)
            {
                if (request.Company_id < 0)
                    return null;

                var userList = await _unitOfWork.DbContext.WorkerContracts
                    .Where(x => x.CompanyId == request.Company_id)
                    .Select(x => x.User).Select(x => x.ToPublicResource()).ToListAsync();

                return userList;
            }
        }
    }
}
