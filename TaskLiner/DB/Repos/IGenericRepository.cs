using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLiner.DB.Repos
{
    /// <summary>
    /// Интерфейс для сущностей, управляемых репозиториями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> : IRepository<T> where T : class
    {

    }

    /// <summary>
    /// Интерфейс предоставляющий необходимый функционал репозитория для объектов типа T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IDisposable where T : class
    {
        public ICollection<T> GetAll();
        public T GetById(int id);
        public void Remove(T item);
        public T Create(T newItem);
        public void Update(T replacedItem);
        public void Save();
        public void SaveAsync();
    }
}
