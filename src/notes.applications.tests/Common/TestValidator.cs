using FluentValidation;
using notes.application.Extensions;

namespace notes.applications.tests.Common
{
    public class TestValidator : AbstractValidator<TestModel>
    {
        public TestValidator()
        {
            RuleFor(x => x.Text).TextValidationRule();

            RuleFor(x => x.Title).TitleValidationRule();

            RuleFor(x => x.Email).EmailValidationRule();

            RuleFor(x => x.Password).PasswordValidationRule();

            RuleFor(x => x.Name).NameValidationRule();

            RuleForEach(t => t.Tags)
                .NotNull()
                .NotEmpty()
                .StringValidationRule(5, 20);

            RuleFor(x => x.Tags)
                .TagsValidationRule();
        }
    }
}

