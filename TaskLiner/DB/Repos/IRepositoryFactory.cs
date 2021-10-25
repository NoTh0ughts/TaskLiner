using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLiner.DB.Repos
{
    public interface IRepositoryFactory
    {
        public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) 
            where TEntity : class;
    }
}
