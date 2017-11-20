using BussinessCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;


namespace MyGenericUnitOfWork.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private DbContext _context;
        private Dictionary<Type, dynamic> _dictRepositories;
        private DbContextTransaction _transaction;

        public UnitOfWork(DbContext context, params dynamic[] repositories)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (repositories == null || repositories.Length == 0)
                throw new ArgumentNullException("repository");

            _context = context;
            _dictRepositories = new Dictionary<Type, dynamic>();
            foreach (var item in repositories)
            {
                if (item == null)
                    throw new ArgumentNullException("repository");

                if (!_dictRepositories.ContainsKey(item.EntityType))
                    _dictRepositories.Add(item.EntityType, item);
            }
        }

        private dynamic this[Type type]
        {
            get
            {
                dynamic iRepository;
                _dictRepositories.TryGetValue(type, out iRepository);
                return iRepository;
            }
            set
            {
                _dictRepositories.Add(type, value);
            }
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            IRepository<T> rep = this[typeof(T)];
            return rep;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_context.Database.Connection.State != ConnectionState.Open)
            {
                _context.Database.Connection.Open();
            }

            _transaction = _context.Database.BeginTransaction(isolationLevel);
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    _dictRepositories.Clear();
                    if (_context != null && _context.Database.Connection.State == ConnectionState.Open)
                    {
                        _context.Database.Connection.Close();
                    }
                }
                catch (ObjectDisposedException)
                {
                }

                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            _disposed = true;
        }
    }
}