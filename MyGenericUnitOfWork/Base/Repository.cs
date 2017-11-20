using BussinessCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected DbContext _context;
        protected DbSet<T> _entities;

        public Repository(MyAppContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public Type EntityType
        {
            get
            {
                return typeof(T);
            }
        }

        public List<T> GetAll()
        {
            return _entities.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public T Get(long id)
        {
            return _entities.SingleOrDefault(i => i.Id == id);
        }

        public async Task<T> GetAsync(long id)
        {
            return await _entities.SingleOrDefaultAsync(i => i.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Entry(entity).State = EntityState.Modified;
        }

        public T Delete(long id)
        {
            T t = Get(id);
            if (t != default(T))
            {
                _entities.Remove(t);
                return t;
            }

            return default(T);
        }

        public async Task<T> DeleteAsync(long id)
        {
            T t = await GetAsync(id);

            if (t != default(T))
            {
                _entities.Remove(t);
                return t;
            }

            return default(T);
        }

        public void Reload(T entity)
        {
            _context.Entry<T>(entity).Reload();
        }

        public async Task ReloadAsync(T entity)
        {
            await _context.Entry<T>(entity).ReloadAsync();
        }

    }
}