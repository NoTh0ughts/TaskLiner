using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Projects
{
    public class GetProjectsForUserQuery : IRequest<IEnumerable<Project>>
    {
        public int UserId { get; set; }

        public class GetProjectsForUserQueryHandler : IRequestHandler<GetProjectsForUserQuery, IEnumerable<Project>>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetProjectsForUserQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<Project>> Handle(GetProjectsForUserQuery request, CancellationToken cancellationToken)
            {
                var projects = await _unitOfWork.DbContext.Projects
                                            .Where(project => project.ProjectAccesses
                                                    .Any(access => access.Contract.UserId == request.UserId))
                                            .Include(project => project.Company)
                                            .ToListAsync(cancellationToken);
                return projects;
            }
        }
    }
}
