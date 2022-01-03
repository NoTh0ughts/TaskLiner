using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Repos;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Projects
{
    public class PostProjectCommand : IRequest<Project>
    {
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Description { get; set; }
        public int Company_Id { get; set; }

        public PostProjectCommand()
        {

        }

        public class PostProjectCommandHandler : IRequestHandler<PostProjectCommand, Project>
        {
            private readonly IGenericRepository<Project> _repository;
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;
            public PostProjectCommandHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _repository = _unitOfWork.GetRepository<Project>();
            }

            public async Task<Project> Handle(PostProjectCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.Name) is true)
                    return null;

                var project = _repository.Create(new Project
                {
                    Name = request.Name,
                    Scope = request.Scope,
                    Description = request.Description,
                    CompanyId = request.Company_Id
                });

                return project;
            }
        }
    }
}
