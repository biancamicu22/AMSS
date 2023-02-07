using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using MMSAPI.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace MMSApi.Tests.Respositories
{
    public class BaseRepositoryTests
    {

        private BaseRepository<TestEntityClass> BaseRespositoryObject;
        private static List<TestEntityClass> Entities;
        private int InitialEntities = 10;

        private static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns((object[] param) => queryable.FirstOrDefault(x => (x as TestEntityClass).Id == param[0].ToString()));

            dbSet.Setup(m => m.Add(It.IsAny<T>()))
                .Returns((T entity) =>
                {
                    Entities.Add(entity as TestEntityClass);
                    return null;
                });

            return dbSet.Object;
        }
        public BaseRepositoryTests()
        {
            Entities = new List<TestEntityClass>();
            for(int i = 0; i < InitialEntities; i++)
            {
                Entities.Add(new TestEntityClass(i.ToString(), $"entity_{i}"));
            }

            var mockedDbContext = new Mock<MMSContext>();
            mockedDbContext.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            mockedDbContext.Setup(x => x.Set<TestEntityClass>())
                .Returns(() => GetQueryableMockDbSet(Entities.ToArray()));

            mockedDbContext.Setup(x => x.Remove(It.IsAny<TestEntityClass>()))
                .Returns((TestEntityClass entity) =>
                {
                    var exists = Entities.FindIndex(e => e.Id == entity.Id);
                    if (exists == -1) throw new Exception();
                    Entities.Remove(entity);
                    return null;
                });

            BaseRespositoryObject = new BaseRepository<TestEntityClass>(mockedDbContext.Object);
        }


        [Fact]
        public void DeleteEntity()
        {
            //Arrange
            var initialCount = Entities.Count;
            //Act
            BaseRespositoryObject.Delete("1");
            //Assert
            Assert.Equal(Entities.Count, initialCount - 1);
        }

        [Fact]
        public void AddEntity()
        {
            //Arrange
            var initialCount = Entities.Count;
            //Act
            BaseRespositoryObject.Add(new TestEntityClass("someNew", "entity_new"));
            //Assert
            Assert.Equal(Entities.Count, initialCount + 1);
        }

        [Fact]
        public void GetAll()
        {
            //Act
            var allEntities = BaseRespositoryObject.GetAll();
            //Assert
            Assert.Equal(Entities.Count, allEntities.Count);
        }

        [Theory]
        [InlineData("0", "entity_0")]
        [InlineData("1", "entity_1")]
        [InlineData("2", "entity_2")]
        public void GetById(string id, string expectedName)
        {
            //Act
            var entity = BaseRespositoryObject.GetById(id);
            //Assert
            Assert.Equal(expectedName, entity.Name);
        }
    }
}
