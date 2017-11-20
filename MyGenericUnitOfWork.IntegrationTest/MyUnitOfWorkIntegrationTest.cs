using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGenericUnitOfWork.Base;
using BussinessCore.Model;
using MyGenericUnitOfWork;
using System.Linq;
using System.Transactions;
using FluentAssertions;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.IntegrationTest
{
    [TestClass]
    public class MyUnitOfWorkIntegrationTest
    {
        private MyAppContext _context;
        private UnitOfWork _db;
        private TransactionScope _transactionScope;


        [TestInitialize]
        public void SetUp()
        {
            DatabaseHelper.MigrateDbToLatest();
            DatabaseHelper.Seed();

            _transactionScope = new TransactionScope();
            _context = new MyAppContext();
            _db = new UnitOfWork(_context, new ClientRepository(_context), new ProductRepository(_context));
        }

        [TestCleanup]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        [TestMethod]
        public void CreateUnitOfWork_ShouldContainsAllRepositories()
        {
            Assert.IsNotNull(_db.Repository<Client>());
            Assert.IsNotNull(_db.Repository<Product>());
        }

        [TestMethod]
        public void CreateUnitOfWork_RetriveClient_ShouldWork()
        {
            Client u = _db.Repository<Client>().Get(1);
            Assert.IsNotNull(u);
            Assert.AreEqual("Joe".ToLower(), u.ClientName.ToLower());
        }

        [TestMethod]
        public void CreateUnitOfWork_ChangeClientName_ShouldWork()
        {
            string newName = "JH1";
            int clientId = 2;
            Client u = _db.Repository<Client>().Get(clientId);
            u.ClientName = newName;
            _db.Repository<Client>().Update(u);
            _db.SaveChanges();

            Client uc = _db.Repository<Client>().Get(clientId);

            Assert.IsNotNull(uc);
            Assert.AreEqual(newName, uc.ClientName);
        }

        [TestMethod]
        public void CreateUnitOfWorkWithTwoRepository_WhenBothUpdateWithTransactionWithoutException_BothChangeShouldBeUpdateToDB()
        {
            _db.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            string newName = "John";
            int clientId = 2;
            Client client = _db.Repository<Client>().Get(clientId);
            client.ClientName = newName;
            _db.SaveChanges();

            int productId = 2;
            string pName = "Milk2017";
            Product product = _db.Repository<Product>().Get(productId);
            product.Name = pName;

            _db.SaveChanges();
            _db.Commit();

            Client c = _db.Repository<Client>().Get(clientId);
            Assert.AreEqual(newName, c.ClientName);

            Product u = _db.Repository<Product>().Get(productId);
            Assert.AreEqual(pName, u.Name);
        }

        [TestMethod]
        public void CreateUnitOfWorkWithTwoRepository_WhenBothUpdateWithTransactionWithException_BothChangeShouldNotBeUpdateToDB()
        {
            string newName = "Barry_123";
            int clientId = 1;
            int productId = 1;
            string pName = "Milk_123";

            try
            {
                _db.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                Client client = _db.Repository<Client>().Get(clientId);
                client.ClientName = newName;
                _db.SaveChanges();

                Product product = _db.Repository<Product>().Get(-productId);
                product.Name = pName;

                _db.SaveChanges();
                _db.Commit();

            }
            catch (Exception)
            {
                _db.Rollback();

                Client c = _db.Repository<Client>().Get(clientId);
                _db.Repository<Client>().Reload(c);
                Assert.AreNotEqual(newName, c.ClientName);

                Product u = _db.Repository<Product>().Get(productId);
                _db.Repository<Product>().Reload(u);
                Assert.AreNotEqual(pName, u.Name);
            }
        }

        [TestMethod]
        public void CreateUnitOfWorkWithTwoRepository_ClientRepository_GetAllClientsSortByName_ShouldWork()
        {
            ClientRepository clientRepoitory = _db.Repository<Client>() as ClientRepository;

            var clients = clientRepoitory.GetAllClientsSortByName();

            clients.Should().HaveCount(DatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

        [TestMethod]
        public async Task CreateUnitOfWorkWithTwoRepository_ClientRepository_GetAllClientsSortByNameAsync_ShouldWork()
        {
            ClientRepository clientRepoitory = _db.Repository<Client>() as ClientRepository;

            var clients = await clientRepoitory.GetAllClientsSortByNameAsync();

            clients.Should().HaveCount(DatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

    }
}
