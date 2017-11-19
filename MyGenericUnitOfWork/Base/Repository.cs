using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext _context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(MyAppContext context)
        {
            _context = context;
            entities = context.Set<T>();
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
            return entities.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        public T Get(long id)
        {
            return entities.SingleOrDefault(i => i.Id == id);
        }

        public async Task<T> GetAsync(long id)
        {
            return await entities.SingleOrDefaultAsync(i => i.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
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
                entities.Remove(t);
                return t;
            }

            return default(T);
        }

        public async Task<T> DeleteAsync(long id)
        {
            T t = await GetAsync(id);

            if (t != default(T))
            {
                entities.Remove(t);
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