using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGenericUnitOfWork.Base;
using System.Transactions;
using FluentAssertions;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.IntegrationTest
{
    [TestClass]
    public class RepositoryIntegrationTest
    {
        private MyAppContext _context;
        private TransactionScope _transactionScope;


        [TestInitialize]
        public void SetUp()
        {
            DatabaseHelper.MigrateDbToLatest();
            DatabaseHelper.Seed();

            _transactionScope = new TransactionScope();
            _context = new MyAppContext();
        }

        [TestCleanup]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        [TestMethod]
        public void ClientRepository_GetAllClientsSortByName_ShouldWork()
        {
            ClientRepository clientRepoitory = new ClientRepository(_context);

            var clients = clientRepoitory.GetAllClientsSortByName();

            clients.Should().HaveCount(DatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

        [TestMethod]
        public async Task ClientRepository_GetAllClientsSortByNameAsync_ShouldWork()
        {
            ClientRepository clientRepoitory = new ClientRepository(_context);

            var clients = await clientRepoitory.GetAllClientsSortByNameAsync();

            clients.Should().HaveCount(DatabaseHelper.ListClient.Count);
            clients.Should().BeInAscendingOrder(c => c.ClientName);
        }

    }
}
