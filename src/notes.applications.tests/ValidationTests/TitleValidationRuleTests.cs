using FluentValidation.TestHelper;
using notes.applications.tests.Common;
using NUnit.Framework;

namespace notes.applications.tests.ValidationTests
{
    [TestFixture(Category = "Unit")]
    public class TitleValidationRuleTests : TestsBase
    {
        private TestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new TestValidator();
            _fixture = CreateFixture();
        }


        [TestCase(5, 200)]

        public void TitleValidationRule_Should_Not_Have_An_Errors(int minLength, int maxLength)
        {
            for (int length = minLength; length <= maxLength; ++length)
            {
                string title = GetStringWithLength(length);
                _validator.ShouldNotHaveValidationErrorFor(x => x.Title, title);
            }
        }

        [TestCase(5, 200)]
        public void TitleValidationRule_Should_Have_An_Error(int minLength, int maxLength)
        {
            for (int length = 0; length < minLength; ++length)
            {
                string title = GetStringWithLength(length);
                _validator.ShouldHaveValidationErrorFor(x => x.Title, title);
            }

            for (int length = maxLength + 1; length < maxLength + 2; ++length)
            {
                string title = GetStringWithLength(length);
                _validator.ShouldHaveValidationErrorFor(x => x.Title, title);
            }
        }
    }
}