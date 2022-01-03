using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Companys
{
    public class GetCompanyQuery : IRequest<Company>
    {
        public int Company_id { get; set; }

        public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, Company>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetCompanyQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Company> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
            {
                if (request.Company_id < 0)
                    return null;

                return await _unitOfWork.DbContext.Companies.FirstOrDefaultAsync(x => x.Id == request.Company_id, 
                    cancellationToken: cancellationToken);
            }
        }
    }
}
