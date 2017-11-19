using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Infrastructure
{
    public interface IRepository<T> where T : BaseEntity
    {
        Type EntityType { get; }
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        T Get(long id);
        Task<T> GetAsync(long id);
        void Insert(T entity);
        void Update(T entity);
        T Delete(long id);
        Task<T> DeleteAsync(long id);
        void Reload(T entity);
        Task ReloadAsync(T entity);
    }
}
