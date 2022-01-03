using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Companys
{
    public class PostCompanyCommand : IRequest<Company>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public class PostCompanyCommandHandler : IRequestHandler<PostCompanyCommand, Company>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public PostCompanyCommandHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Company> Handle(PostCompanyCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _unitOfWork.DbContext.Companies.AnyAsync(x => x.Name == request.Name);
                
                if(isExists is false)
                {
                    var newCompany = _unitOfWork.GetRepository<Company>().Create(new Company
                    { Name = request.Name, Description = request.Description });
                    return newCompany;
                }

                return null;
            }
        }
    }
}
