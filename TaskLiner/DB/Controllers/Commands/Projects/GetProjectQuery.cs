using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Repos;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Projects
{
    public class GetProjectQuery : IRequest<Project>
    {
        public int Id { get; set; }
        public int User_id { get; set; }

        public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, Project>
        {
            private readonly IGenericRepository<Project> _repository;
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;
            public GetProjectQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _repository = _unitOfWork.GetRepository<Project>();
            }

            public async Task<Project> Handle(GetProjectQuery request, CancellationToken cancellationToken)
            {
                bool isAccessibly = await (from user in _unitOfWork.DbContext.Users
                                           join contract in _unitOfWork.DbContext.WorkerContracts on user.Id equals contract.UserId
                                           join company in _unitOfWork.DbContext.Companies on contract.CompanyId equals company.Id
                                           join project in _unitOfWork.DbContext.Projects on company.Id equals project.CompanyId
                                           where user.Id == request.User_id && project.Id == request.Id
                                           select project)
                                           .AnyAsync(cancellationToken);
                
                if (isAccessibly is not true)
                    return null;

                var task = _repository.GetById(request.Id);

                return task;
            }
        }
    }
}
