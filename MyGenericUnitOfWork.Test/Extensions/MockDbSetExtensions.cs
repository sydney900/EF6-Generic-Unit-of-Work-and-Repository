using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFTests.Extension
{
    public static class MockDbSetExtensions
    {
        public static void SetMockData<T>(this Mock<DbSet<T>> mockSet, IList<T> mockData) where T : class
        {
            var data = mockData.AsQueryable();

            SetupMockDbSetForQuery<T>(mockSet, data);
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(t => 
            { 
                mockData.Add(t);
                data = mockData.AsQueryable();
                SetupMockDbSetForQuery<T>(mockSet, data);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t =>
            {
                mockData.Remove(t);
                data = mockData.AsQueryable();
                SetupMockDbSetForQuery<T>(mockSet, data);
            });
        }

        private static void SetupMockDbSetForQuery<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        public static void SetMockDataAsync<T>(this Mock<DbSet<T>> mockSet, IList<T> mockData) where T : class
        {
            var data = mockData.AsQueryable();

            SetupMockDbSetAsyncForQuery<T>(mockSet, data);
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(t =>
            {
                mockData.Add(t);
                data = mockData.AsQueryable();
                SetupMockDbSetAsyncForQuery<T>(mockSet, data);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t =>
            {
                mockData.Remove(t);
                data = mockData.AsQueryable();
                SetupMockDbSetAsyncForQuery<T>(mockSet, data);
            });

        }

        private static void SetupMockDbSetAsyncForQuery<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
        {
            mockSet.As<IDbAsyncEnumerable<T>>()
                           .Setup(m => m.GetAsyncEnumerator())
                           .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

    }
}
