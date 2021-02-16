using AutoFixture;
using Moq;
using notes.application.Exceptions;
using notes.application.Extensions;
using notes.application.Interfaces;
using notes.application.Models.User;
using notes.application.Services;
using notes.application.tests.Common;
using notes.data.Entities;
using notes.data.Interfaces;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace notes.application.tests.ServiceTests
{
    [TestFixture(Category = "Unit")]
    public class UserServiceTests : TestsBase
    {
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _fixture = CreateFixture();
            _mapper = CreateMapper();

            _userRepositoryMock = new Mock<IUserRepository>();

            _userService = new UserService(_mapper, _userRepositoryMock.Object);
        }

        [Test]
        public async Task UserService_GetUserAsync_OK()
        {
            Guid userId = Guid.NewGuid();
            _userRepositoryMock.Setup(t => t.FindOneAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    null))
                .ReturnsAsync(() => new User
                {
                    Id = userId
                });

            //Act
            var model = await _userService.GetUserAsync(userId);

            //Assert
            Assert.AreEqual(model.Id, userId);
        }

        [Test]
        public void UserService_ChangeUserPasswordAsync_User_Not_Found()
        {
            string email = _fixture.Create<string>();

            var changePasswordModel = _fixture
                .Build<ChangePasswordModel>()
                .With(t => t.Email, email)
                .Create();

            var exception = Assert.ThrowsAsync<AppException>(
                async () => await _userService.ChangeUserPasswordAsync(changePasswordModel));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.EntityNotFound);
            Assert.AreEqual(exception.EntityId, email);
        }

        [Test]
        public void UserService_ChangeUserPasswordAsync_Password_Is_Incorrect()
        {
            var user = MockUser();

            var model = _fixture
                .Build<ChangePasswordModel>()
                .With(u => u.Email, user.Email)
                .Create();

            var exception = Assert.ThrowsAsync<AppException>(
                async () => await _userService.ChangeUserPasswordAsync(model));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.UserPasswordIsIncorrect);
            Assert.AreEqual(exception.EntityId, user.Id);
        }

        [Test]
        public async Task UserService_ChangeUserPasswordAsync_OK()
        {
            string pass = "123";
            var user = MockUser(pass);
            Guid modifierId = Guid.NewGuid();
            string newPass = "345";
            DataExtensions.CreatePasswordHash(newPass, out byte[] newHash, out byte[] newSalt);

            var changeModel = _fixture.Build<ChangePasswordModel>()
                .With(u => u.Email, user.Email)
                .With(t => t.OldPassword, pass)
                .With(t => t.NewPassword, newPass)
                .Create();

            await _userService.ChangeUserPasswordAsync(changeModel);

            _userRepositoryMock.Verify(t => t.InsertOrUpdateAsync(It.Is<User>(u => u.Id == user.Id), modifierId));
        }
    }
}