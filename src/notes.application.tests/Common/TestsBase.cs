using AutoFixture;
using AutoMapper;
using Moq;
using notes.application.Extensions;
using notes.application.Mapper;
using notes.data.Entities;
using notes.data.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace notes.application.tests.Common
{
    public class TestsBase
    {
        protected Fixture _fixture;
        protected IMapper _mapper;
        protected Mock<IUserRepository> _userRepositoryMock;
        protected Mock<INoteRepository> _noteRepositoryMock;
        protected Mock<ITagRepository> _tagRepositoryMock;

        internal Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }

        internal IMapper CreateMapper()
        {
            var mappingConfig = new MapperConfiguration(
                mc => { mc.AddProfile(new AppMappingProfile()); }
            );

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        protected User MockUser(string pass = "defaultPassword")
        {
            DataExtensions.CreatePasswordHash(pass, out byte[] hash, out byte[] salt);

            var user = _fixture.Build<User>()
                .With(t => t.PasswordHash, hash)
                .With(t => t.PasswordSalt, salt)
                .Create();

            _userRepositoryMock
                .Setup(t => t.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(user);

            return user;
        }

        protected string GetStringWithLength(int leng)
        {
            return string.Join("", _fixture.CreateMany<string>((leng / 36) + 1)).Substring(0, leng);
        }
    }
}
