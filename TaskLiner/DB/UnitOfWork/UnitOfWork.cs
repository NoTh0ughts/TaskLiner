using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TaskLiner.DB.Repos;

namespace TaskLiner.DB.UnitOfWork
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext
    {
        private bool _disposed;
        private Dictionary<Type, object> _repositories;
        private IRepositoryFactory _repositoryFactoryImplementation;

        public UnitOfWork(TContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TContext DbContext { get; }


        public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            _repositories ??= new Dictionary<Type, object>();

            if (hasCustomRepository)
            {
                var customRepo = DbContext.GetService<IGenericRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(DbContext);
            }

            return _repositories[type] as IGenericRepository<TEntity>;
        }


        public int SaveChanges(bool ensureAutoHistory = false)
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            return await DbContext.SaveChangesAsync();
        }



        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
        {
            using (var ts = new TransactionScope())
            {
                var count = 0;
                foreach (var unitOfWork in unitOfWorks)
                {
                    count += await unitOfWork.SaveChangesAsync(ensureAutoHistory);
                }

                count += await SaveChangesAsync(ensureAutoHistory);

                ts.Complete();

                return count;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    DbContext.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
