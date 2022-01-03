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
using TaskEntity = TaskLiner.DB.Entity.Task;

namespace TaskLiner.DB.Controllers.Commands.Task
{
    public class PostTaskCommand : IRequest<TaskEntity>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CheckList { get; set; }
        public string Content { get; set; }
        public string State { get; set; }
        public string Column_state { set; get; }
        public DateTime SpendedTime { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Project_Id { get; set; }

        public class PostTaskCommandHandler : IRequestHandler<PostTaskCommand, TaskEntity>
        {
            private readonly IGenericRepository<TaskEntity> _repository;
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public PostTaskCommandHandler(IGenericRepository<TaskEntity> repository, 
                                          IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _repository = _unitOfWork.GetRepository<TaskEntity>();
            }

            public async Task<TaskEntity> Handle(PostTaskCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _unitOfWork.DbContext.Projects.AnyAsync(x => x.Id == request.Project_Id, 
                                                                        cancellationToken: cancellationToken);

                if(isExists is true)
                {
                    var newTask = _repository.Create(new TaskEntity
                    {
                        Name        = request.Name,
                        Description = request.Description,
                        Checklist   = request.CheckList,
                        Content     = request.Content,
                        State       = request.State,
                        ColumnState = request.State,
                        SpendedTime = request.SpendedTime,
                        AddDate     = request.AddDate,
                        StartDate   = request.StartDate,
                        EndDate     = request.EndDate,
                        ProjectId   = request.Project_Id
                    });

                    return newTask;
                }

                return null;
            }
        }
    }
}
