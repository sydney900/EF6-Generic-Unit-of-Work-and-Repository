using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyGenericUnitOfWork.Base;
using BussinessCore.Model;
using System.Linq;

namespace MyGenericUnitOfWork.Test
{
    [TestClass]
    public class MyUnitWorkUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithoutDbContext_ShouldThrowException()
        {
            UnitOfWork unitOfWork = new UnitOfWork(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithoutRepositories_ShouldThrowException()
        {
            UnitOfWork unitOfWork = new UnitOfWork(new MyAppContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithNullRepositories_ShouldThrowException()
        {
            UnitOfWork unitOfWork = new UnitOfWork(new MyAppContext(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUnitOfWork_WithOneNullRepository_ShouldThrowException()
        {
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>();
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, new ClientRepository(mockAppContext.Object), null);
        }

        [TestMethod]
        public void CreateUnitOfWork_WithAnyClassRepository_ShouldGetThisTypeOfRepository()
        {
            Mock<MyAppContext> mockAppContext = new Mock<MyAppContext>();
            Repository<Client> rep = new Repository<Client>(mockAppContext.Object);
            UnitOfWork unitOfWork = new UnitOfWork(mockAppContext.Object, rep);

            Assert.IsInstanceOfType(unitOfWork.Repository<Client>(), typeof(Repository<Client>));
        }
    }
}
