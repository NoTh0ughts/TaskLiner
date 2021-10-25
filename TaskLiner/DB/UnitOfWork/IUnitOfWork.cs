using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskLiner.DB.Repos;

namespace TaskLiner.DB.UnitOfWork
{
    /// <summary>
    /// Определяет интерфейс для обобщенного IUnitOfWork
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext DbContext { get; }

        /// <summary>
        /// Сохраняет все изменения для контекста
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c>При обновлении данных обеспечить автозапись истории</param>
        /// <param name="unitOfWorks">Опциональный <see cref="IUnitOfWork"/> массив объектов, которые так же необходимо обновить</param>
        /// <returns>A <see cref="Task{TResult}"/> Представляет ассинхронную операцию сохранения.Результат возвращает количество затронутых сущностей в бд.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks);
    }
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Возвращает репозиторий для сущности... <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> Поддерживает ли кастомный репозиторий </param>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <returns> Объект, унаследовавший <see cref="IGenericRepository{TEntity}"/> интерфейс.</returns>
        IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

        /// <summary>
        /// Сохраняет все изменения в текущем контексте.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> При обновлении данных обеспечить автозапись истории</param>
        /// <returns>Количество затронутых объектов БД</returns>
        int SaveChanges(bool ensureAutoHistory = false);

        /// <summary>
        /// Ассинхронно сохраняет все изменения в текущем контексте
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> При обновлении данных обеспечить автозапись истории</param>
        /// <returns>A <see cref="Task{TResult}"/> Представляет ассинхронную операцию сохранения.Результат возвращает количество затронутых сущностей в бд</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);
    }
}
