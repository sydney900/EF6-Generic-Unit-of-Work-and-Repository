using BussinessCore.Model;
using MyGenericUnitOfWork.Base;
using System.Data.Entity;
using System.Data;

namespace MyGenericUnitOfWork
{
    public partial class MyAppContext : DbContext, IAppContext
    {
        public MyAppContext()
            : base("name=MyAppEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().Property(x => x.Id).HasColumnName("ClientId");
            //modelBuilder.Entity<Client>().HasKey<long>(x => x.Id).Property(x => x.Id).HasColumnName("ClientId").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);
            modelBuilder.Entity<Client>().Property(x => x.Timestamp).IsRowVersion();

            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("ProductId");
            modelBuilder.Entity<Product>().Property(x => x.TimeStamp).IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }

        private DbContextTransaction _transaction;
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (Database.Connection.State != ConnectionState.Open)
            {
                Database.Connection.Open();
            }

            _transaction = Database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            if (_transaction != null)
                _transaction.Commit();
        }

        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }

        public void CloseConnection()
        {
            if (Database.Connection.State == ConnectionState.Open)
            {
                Database.Connection.Close();
            }
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}