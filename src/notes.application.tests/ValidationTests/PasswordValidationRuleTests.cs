using FluentValidation.TestHelper;
using notes.application.Exceptions;
using notes.application.tests.Common;
using NUnit.Framework;

namespace notes.application.tests.ValidationTests
{
    [TestFixture(Category = "Unit")]
    public class PasswordValidationRuleTests
    {
        private TestValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new TestValidator();
        }

        [TestCase("Password123!")]
        [TestCase("Password123!")]
        [TestCase("Password123!")]
        [TestCase("a!!!!!!1111AA")]
        [TestCase("a!__1234AABC")]
        public void Password_ValidationRule_Should_Not_Have_An_Errors(string password)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, password);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("1")]
        [TestCase("12")]
        [TestCase("123")]
        [TestCase("1234")]
        [TestCase("12345")]
        [TestCase("123456")]
        [TestCase("1234567")]
        [TestCase("123456789012345678901")]
        [TestCase("passwordpass")]
        [TestCase("!!!!!!@@@@@@")]
        [TestCase("A!!!!!!1111AA")]
        public void Password_ValidationRule_Should_Have_An_Error(string password)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Password, password)
                .WithErrorCode(ErrorCodes.PasswordFormatIsNotValid);
        }
    }
}