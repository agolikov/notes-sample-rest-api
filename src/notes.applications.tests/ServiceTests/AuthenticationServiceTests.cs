using System.Linq.Expressions;
using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using notes.application.Exceptions;
using notes.application.Models.User;
using notes.application.Services;
using notes.applications.tests.Common;
using notes.applications.tests.Extensions;
using notes.data.Entities;
using notes.data.Interfaces;
using NUnit.Framework;

namespace notes.applications.tests.ServiceTests
{
    [TestFixture(Category = "Unit")]
    public class AuthenticationServiceTests : TestsBase
    {
        private AuthenticationService _authenticationService;
        private Mock<IOptions<application.Settings.Settings>> _settingsMock;

        [SetUp]
        public void MockSetup()
        {
            _fixture = CreateFixture();
            _mapper = CreateMapper();

            _settingsMock = new Mock<IOptions<application.Settings.Settings>>();

            _settingsMock.Setup(t => t.Value).Returns(new application.Settings.Settings
            {
                SecretKey = "f1b95a6a-d537-4cff-94a4-2772ed87d209"
            });
            
            _authenticationService =
                new AuthenticationService(_mapper, UserRepositoryMock.Object,
                    new application.Settings.Settings
                    {
                        SecretKey = Guid.NewGuid().ToString()
                    });
        }

        [Test]
        public async Task AuthenticationService_SignUpAsync_OK()
        {
            var signUpModel = _fixture.Build<SignUpModel>()
                .Create();

            await _authenticationService.SignUpAsync(signUpModel);

            UserRepositoryMock.Verify(t =>
                t.InsertOrUpdateAsync(It.Is<User>(
                    t => t.Email == signUpModel.Email), It.IsAny<Guid>()));
        }

        [Test]
        public void AuthenticationService_SignUpAsync_Email_Already_Taken()
        {
            var user = MockUser();

            var signUpModel = _fixture.Build<SignUpModel>()
                .With(t => t.Email, user.Email)
                .Create();

            UserRepositoryMock
                .Setup(t => t.FindOneAsync(It.Is<Expression<Func<User, bool>>>(
                    t => t.ToString().Contains("Email"))))
                .ReturnsAsync(user);

            //Act
            var exception = Assert.ThrowsAsync<AppException>(async () => await _authenticationService.SignUpAsync(signUpModel)); ;

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.UserEmailAlreadyTaken);
            Assert.AreEqual(exception.EntityId, user.Email);
        }

        [Test]
        public async Task AuthenticationService_SignInAsync_OK()
        {
            string password = "password";
            var user = MockUser(pass: password);

            var signInModel = _fixture.Build<SignInModel>()
                .With(t => t.Email, user.Email)
                .With(t => t.Password, password)
                .Create();

            UserRepositoryMock
                .Setup(t => t.FindOneAsync(It.Is<Expression<Func<User, bool>>>(
                    t => t.ToString().Contains("UserName"))))
                .ReturnsAsync(user.ShallowCopy());

            //Act
            var result = await _authenticationService.SignInAsync(signInModel);

            //Assert
            Assert.IsNotEmpty(result.Token);
            Assert.AreEqual(result.User.Email, user.Email);
        }


        [Test]
        public void AuthenticationService_SignInAsync_User_Name_Not_Found()
        {
            var signInModel = _fixture.Build<SignInModel>()
                .Create();

            //Act
            var exception = Assert.ThrowsAsync<AppException>(
                async () => await _authenticationService.SignInAsync(signInModel));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.EmailNotFound);
            Assert.AreEqual(exception.EntityId, signInModel.Email);
        }

        [Test]
        public void AuthenticationService_SignInAsync_Password_Is_Incorrect()
        {
            string password = "password";
            var user = MockUser(pass: password);

            var signInModel = _fixture.Build<SignInModel>()
                .With(t => t.Email, user.Email)
                .Create();

            UserRepositoryMock
                .Setup(t => t.FindOneAsync(It.Is<Expression<Func<User, bool>>>(
                    t => t.ToString().Contains("UserName"))))
                .ReturnsAsync(user.ShallowCopy());

            //Act
            var exception = Assert.ThrowsAsync<AppException>(
                async () => await _authenticationService.SignInAsync(signInModel));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.UserPasswordIsIncorrect);
            Assert.AreEqual(exception.EntityId, user.Email);
        }
    }
}

