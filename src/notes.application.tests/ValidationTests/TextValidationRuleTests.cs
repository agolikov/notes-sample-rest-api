using FluentValidation.TestHelper;
using notes.application.tests.Common;
using NUnit.Framework;

namespace notes.application.tests.ValidationTests
{
    [TestFixture(Category = "Unit")]
    public class TextValidationRuleTests : TestsBase
    {
        private TestValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new TestValidator();
            _fixture = CreateFixture();
        }

        [TestCase(10, 2000)]

        public void TextValidationRule_Should_Not_Have_An_Errors(int minLength, int maxLength)
        {
            for (int length = minLength; length <= maxLength; ++length)
            {
                string text = GetStringWithLength(length);
                _validator.ShouldNotHaveValidationErrorFor(x => x.Text, text);
            }
        }

        [TestCase(10, 2000)]
        public void TextValidationRule_Should_Have_An_Error(int minLength, int maxLength)
        {
            for (int length = 0; length < minLength; ++length)
            {
                string text = GetStringWithLength(length);
                _validator.ShouldHaveValidationErrorFor(x => x.Text, text);
            }

            for (int length = maxLength + 1; length < maxLength + 2; ++length)
            {
                string text = GetStringWithLength(length);
                _validator.ShouldHaveValidationErrorFor(x => x.Text, text);
            }
        }
    }
}