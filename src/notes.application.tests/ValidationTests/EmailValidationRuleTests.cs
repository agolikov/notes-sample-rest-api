using FluentValidation.TestHelper;
using notes.application.Exceptions;
using notes.application.tests.Common;
using NUnit.Framework;

namespace notes.application.tests.ValidationTests
{

    [TestFixture(Category = "Unit")]
    public class EmailValidationRuleTests
    {
        private TestValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new TestValidator();
        }

        [TestCase("a@test.com")]
        [TestCase("a.bcde@test.com")]
        [TestCase("email@example.com")]
        [TestCase("firstname.lastname@example.com")]
        [TestCase("email@subdomain.example.com")]
        [TestCase("firstname+lastname@example.com")]
        [TestCase("email@123.123.123.123")]
        [TestCase("email@[123.123.123.123]")]
        [TestCase("1234567890@example.com")]
        [TestCase("email@example-one.com")]
        [TestCase("_______@example.com")]
        [TestCase("email@example.name")]
        [TestCase("email@example.museum")]
        [TestCase("email@example.co.jp")]
        [TestCase("firstname-lastname@example.com")]
        public void Email_ValidationRule_Should_Not_Have_An_Errors(string email)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Email, email);
        }

        [TestCase("@")]
        [TestCase("")]
        [TestCase("very.unusual.”@”.unusual.com@example.comm")]
        [TestCase("very.”(),:;<>[]”.VERY.”very@\\\"very”.unusual@strange.example.com")]
        [TestCase("plainaddress")]
        [TestCase("#@%^%#$@#$@#.com")]
        [TestCase("@example.com")]
        [TestCase("email@example@example.com")]

        public void Email_ValidationRule_Should_Have_An_Error(string email)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Email, email)
                .WithErrorCode(ErrorCodes.EmailFormatIsNotValid);
        }
    }
}