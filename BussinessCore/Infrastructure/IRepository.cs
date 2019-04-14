using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BussinessCore.Infrastructure
{
    public interface IRepository<T> where T : BaseEntity
    {
        Type EntityType { get; }
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        T Get(long id);
        Task<T> GetAsync(long id);
        List<T> Find(Expression<Func<T, bool>> predict);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predict);
        List<T> FindByProperty(string propertyName, object propertyValue);
        Task<List<T>> FindByPropertyAsync(string propertyName, object propertyValue);
        void Insert(T entity);
        void Update(T entity);
        T Delete(long id);
        Task<T> DeleteAsync(long id);
        void Reload(T entity);
        Task ReloadAsync(T entity);
    }
}
