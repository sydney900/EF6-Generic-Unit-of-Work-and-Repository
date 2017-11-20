using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.Base
{
    public interface IAppContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction(IsolationLevel isolationLevel);
        void Commit();
        void Rollback();
        void CloseConnection();
        void Dispose();
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
