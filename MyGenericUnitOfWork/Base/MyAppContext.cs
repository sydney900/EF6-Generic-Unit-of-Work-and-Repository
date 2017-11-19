using Core.Model;
using System.Data.Entity;

namespace MyGenericUnitOfWork.Base
{
    public partial class MyAppContext : DbContext
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

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}