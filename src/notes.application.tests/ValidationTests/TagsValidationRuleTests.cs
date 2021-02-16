using FluentValidation.TestHelper;
using notes.application.Exceptions;
using notes.application.tests.Common;
using NUnit.Framework;
using System.Linq;

namespace notes.application.tests.ValidationTests
{
    [TestFixture(Category = "Unit")]
    public class TagsValidationRuleTests : TestsBase
    {
        private TestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new TestValidator();
            _fixture = CreateFixture();
        }

        [TestCase(5)]

        public void TagsValidationRule_Should_Not_Have_An_Errors(int maxTagsCount)
        {
            for (int tagsCount = 0; tagsCount < maxTagsCount; ++tagsCount)
            {
                string[] tags = Enumerable.Repeat(GetStringWithLength(5), tagsCount).ToArray();
                _validator.ShouldNotHaveValidationErrorFor(x => x.Tags, tags);
            }
        }

        [TestCase(6)]
        public void TagsValidationRule_Should_Have_An_Errors(int maxTagsCount)
        {
            string[] tags = Enumerable.Repeat(GetStringWithLength(5), maxTagsCount).ToArray();

            _validator.ShouldHaveValidationErrorFor(x => x.Tags, tags)
                .WithErrorCode(ErrorCodes.TagsCountIsLarge);
        }

        [TestCase(5, 20)]
        public void Tags_ValidationRule_Should_Not_Have_An_Errors(int minTagLength, int maxTagLength)
        {
            for (int length = minTagLength; length <= maxTagLength; ++length)
            {
                string tag = GetStringWithLength(length);
                string[] tags = { tag };
                _validator.ShouldNotHaveValidationErrorFor(x => x.Tags, tags);
            }
        }

        [TestCase(5, 20)]
        public void Tags_ValidationRule_Should_Have_An_Error(int minTagLength, int maxTagLength)
        {
            for (int length = 0; length < minTagLength; ++length)
            {
                string tag = GetStringWithLength(length);
                string[] tags = { tag };
                _validator.ShouldHaveValidationErrorFor(x => x.Tags, tags);
            }

            for (int length = maxTagLength + 1; length < maxTagLength + 2; ++length)
            {
                string tag = GetStringWithLength(length);
                string[] tags = { tag };
                _validator.ShouldHaveValidationErrorFor(x => x.Tags, tags);
            }
        }
    }
}