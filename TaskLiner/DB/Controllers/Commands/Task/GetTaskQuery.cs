using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Repos;
using TaskLiner.DB.UnitOfWork;
using TaskEntity = TaskLiner.DB.Entity.Task;

namespace TaskLiner.DB.Controllers.Commands.Task
{
    public class GetTaskQuery : IRequest<TaskEntity>
    {
        public int Id { get; set; }

        public class GetTaskCommandHandler : IRequestHandler<GetTaskQuery, TaskEntity>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetTaskCommandHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<TaskEntity> Handle(GetTaskQuery request, CancellationToken cancellationToken)
            {
                return _unitOfWork.GetRepository<TaskEntity>().GetById(request.Id);
            }
        }
    }
}
