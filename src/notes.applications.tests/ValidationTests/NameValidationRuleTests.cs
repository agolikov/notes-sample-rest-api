using FluentValidation.TestHelper;
using notes.applications.tests.Common;
using NUnit.Framework;

namespace notes.applications.tests.ValidationTests
{
    [TestFixture(Category = "Unit")]
    public class NameValidationRuleTests : TestsBase
    {
        private TestValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new TestValidator();
            _fixture = CreateFixture();
        }

        [TestCase(3, 20)]

        public void Name_ValidationRule_Should_Not_Have_An_Errors(int minLength, int maxLength)
        {
            for (int length = minLength; length <= maxLength; ++length)
            {
                string name = GetStringWithLength(length);
            }
        }

        [TestCase(3, 20)]
        public void Name_ValidationRule_Should_Have_An_Error(int minLength, int maxLength)
        {
            for (int length = 0; length < minLength; ++length)
            {
                string name = GetStringWithLength(length);
                _validator.ShouldHaveValidationErrorFor(x => x.Name, name);
            }

            for (int length = maxLength + 1; length < maxLength + 2; ++length)
            {
                string name = GetStringWithLength(length);
                _validator.ShouldHaveValidationErrorFor(x => x.Name, name);
            }
        }
    }
}