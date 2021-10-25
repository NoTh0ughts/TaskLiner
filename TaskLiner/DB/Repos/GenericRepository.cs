using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLiner.DB.Repos
{
    /// <summary>
    /// Класс - репозиторий для классов сущностей
    /// Предоставляет базовые операции данных 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public GenericRepository()
        {

        }


        public void Save() => context.SaveChanges();
        public void SaveAsync() => context.SaveChangesAsync();
        public void Dispose() => context.SaveChanges();
        public ICollection<TEntity> GetAll() => dbSet.ToList();
        public TEntity GetById(int id) => dbSet.Find(id);

        // Удаляет переданный объект сущности, сохраняет состояние контекста
        public void Remove(TEntity item)
        {
            dbSet.Remove(item);
            context.SaveChanges();
        }

        /// <summary>
        /// Создает сущность типа T и возвращает ее, сохраняет состояние контекста
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public TEntity Create(TEntity newItem)
        {
            var newEntity = dbSet.Add(newItem).Entity;
            context.SaveChanges();
            return newEntity;
        }

        /// <summary>
        /// Обновляет данные сущности по ее ID, сохраняет состояние контекста
        /// </summary>
        /// <param name="replacedItem"></param>
        public void Update(TEntity replacedItem)
        {
            context.Entry(replacedItem).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
