using AutoFixture;
using Microsoft.EntityFrameworkCore;
using notes.data.Exceptions;
using notes.data.tests.Test;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace notes.data.tests.Repositories
{
    [TestFixture(Category = "Unit")]
    public class RepositoryTests
    {
        private Fixture _fixture;
        private TestRepository _testRepository;
        private TestDbContext _dbContext;

        [SetUp]
        public void MockSetup()
        {
            _fixture = new Fixture();
            _fixture
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TestDbContext(options);

            _testRepository = new TestRepository(_dbContext);
        }

        [Test]
        public async Task Repository_FindOneAsync_OK()
        {
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();
            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            var entity = await _testRepository.FindOneAsync(t => t.Id == testEntity.Id);
            Assert.That(entity,Is.SameAs(testEntity));
        }

        [Test]
        public async Task Repository_FindOneAsync_DeletedEntity_Failed()
        {
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, true)
                .Create();

            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            var entity = await _testRepository.FindOneAsync(t => t.Id == testEntity.Id);
            Assert.That(entity,Is.Null);
        }

        [Test]
        public async Task Repository_FindOneByIdAsync_OK()
        {
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();
            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            var entity = await _testRepository.FindOneAsync(t => t.Id == testEntity.Id);
            Assert.That(testEntity,Is.SameAs(entity));
        }

        [Test]
        public async Task Repository_FindOneByIdAsync_DeletedEntity_Failed()
        {
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, true)
                .Create();

            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            var entity = await _testRepository.FindOneAsync(t => t.Id == testEntity.Id);
            Assert.That(entity, Is.Null);
        }

        [Test]
        public async Task Repository_InsertAsync_OK()
        {
            Guid modifiedBy = Guid.NewGuid();
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();

            var inserted = await _testRepository.InsertOrUpdateAsync(testEntity, modifiedBy);
            
            Assert.That(testEntity,Is.SameAs(inserted));
        }

        [Test]
        public async Task Repository_UpdateAsync_OK()
        {
            Guid modifiedBy = Guid.NewGuid();
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();

            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            var testEntityUpdated = _fixture.Build<TestEntity>()
                .With(t => t.Id, testEntity.Id)
                .With(t => t.Version, testEntity.Version)
                .With(t => t.IsDeleted, false)
                .Create();

            var updated = await _testRepository.InsertOrUpdateAsync(testEntityUpdated, modifiedBy);

            Assert.That(updated,Is.SameAs(testEntityUpdated));
        }

        [Test]
        public async Task Repository_UpdateAsync_Wrong_Version()
        {
            Guid modifiedBy = Guid.NewGuid();
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();

            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            var testEntityUpdated = _fixture.Build<TestEntity>()
                .With(t => t.Id, testEntity.Id)
                .With(t => t.Version, testEntity.Version + 1)
                .With(t => t.IsDeleted, false)
                .Create();

            var exception = Assert.ThrowsAsync<DalException>(async () => await _testRepository.InsertOrUpdateAsync(testEntityUpdated, modifiedBy));

            //Assert
            Assert.That(exception,Is.Not.Null);
            Assert.That(exception.Code,Is.SameAs(ErrorCodes.VersionIsNotCorrect));
            Assert.That(exception.EntityId,Is.EqualTo(testEntityUpdated.Id));
        }

        [Test]
        public async Task Repository_DeleteAsync_Soft_OK()
        {
            Guid modifiedBy = Guid.NewGuid();
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();

            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            await _testRepository.DeleteAsync(testEntity.Id, modifiedBy);

            var deletedEntity = await _dbContext.TestEntities.FirstOrDefaultAsync(t => t.Id == testEntity.Id);

            testEntity.IsDeleted = true;
            Assert.That(deletedEntity,Is.SameAs(testEntity));
        }

        [Test]
        public void Repository_DeleteAsync_Soft_Entity_Not_Found()
        {
            Guid modifiedBy = Guid.NewGuid();
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();

            var exception = Assert.ThrowsAsync<DalException>(async () => await _testRepository.DeleteAsync(testEntity.Id, modifiedBy));

            //Assert
            Assert.That(exception,Is.Not.Null);
            Assert.That(exception.Code,Is.SameAs(ErrorCodes.EntityNotFound));
            Assert.That(exception.EntityId,Is.EqualTo(testEntity.Id));
        }

        [Test]
        public async Task Repository_DeleteAsync_Hard_OK()
        {
            Guid modifiedBy = Guid.NewGuid();
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .Create();
            _dbContext.Add(testEntity);
            await _dbContext.SaveChangesAsync();

            await _testRepository.DeleteAsync(testEntity.Id, modifiedBy, hardDelete: true);

            //Assert
            var deletedEntity = await _dbContext.TestEntities.FirstOrDefaultAsync(t => t.Id == testEntity.Id);

            Assert.That(deletedEntity,Is.Null);
        }

        [Test]
        public async Task Repository_FindAsync_OK()
        {
            int totalCount = 100;
            var testEntity = _fixture.Build<TestEntity>()
                .With(t => t.IsDeleted, false)
                .CreateMany(totalCount);

            await _dbContext.AddRangeAsync(testEntity);
            await _dbContext.SaveChangesAsync();

            int pageSize = 10;
            var entity = await _testRepository.FindManyAsync(0, pageSize, true,
                null, null, null);

            Assert.That(entity,Is.Not.Null);
            Assert.That(entity.Count(),Is.EqualTo(pageSize));
        }
    }
}