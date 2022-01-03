using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Entity.Views;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.DB.Controllers.Commands.Companys
{
    public class GetCompaniesForUserQuery : IRequest<IEnumerable<CompanyResource>>
    {
        public int User_id { get; set; }

        public class GetCompanysForUserQueryHandler : IRequestHandler<GetCompaniesForUserQuery, IEnumerable<CompanyResource>>
        {
            private readonly IUnitOfWork<TaskLinerContext> _unitOfWork;

            public GetCompanysForUserQueryHandler(IUnitOfWork<TaskLinerContext> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<CompanyResource>> Handle(GetCompaniesForUserQuery request, CancellationToken cancellationToken)
            {
                var result = (from contract in _unitOfWork.DbContext.WorkerContracts
                              from company in _unitOfWork.DbContext.Companies.Where(x => x.Id == contract.CompanyId)
                              where contract.UserId == request.User_id
                              select new CompanyResource
                              {
                                  Id = company.Id,
                                  Name = company.Name,
                                  Projects_Names = company.Projects
                                                          .Where(x => x.ProjectAccesses
                                                             .Any(x => x.Contract.UserId == request.User_id))
                                                          .Select(x => x.Name)
                                                          .ToArray(),
                                  Owner = contract.User.Nickname
                              }).AsEnumerable().GroupBy(x => x.Id, (key, group) => group.First());


                return result;
            }
        }
    }
}
