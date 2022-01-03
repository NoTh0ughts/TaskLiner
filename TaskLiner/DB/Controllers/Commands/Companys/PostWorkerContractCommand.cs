using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Repos;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Companys
{
    public class PostWorkerContractCommand : IRequest<WorkerContract>
    {
        public int User_id { get; set; }
        public int Company_id { get; set; }
        public bool IsOwner { get; set; }

        public class PostWorkerContractCommandHandler : IRequestHandler<PostWorkerContractCommand, WorkerContract>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;
            private readonly IGenericRepository<WorkerContract> _repository;
            public PostWorkerContractCommandHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _repository = _unitOfWork.GetRepository<WorkerContract>();
            }

            public async Task<WorkerContract> Handle(PostWorkerContractCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _unitOfWork.DbContext.WorkerContracts
                    .AnyAsync(x => x.UserId == request.User_id && x.CompanyId == request.Company_id);

                if (isExists)
                    return null;

                var newContract = _repository.Create(new WorkerContract()
                {
                    CompanyId = request.Company_id,
                    UserId = request.User_id,
                    IsOwner = request.IsOwner
                });

                _repository.SaveAsync();
                await _unitOfWork.SaveChangesAsync();
                return newContract;
            }
        }
    }
}
