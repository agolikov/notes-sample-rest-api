using FluentValidation;
using FluentValidation.Results;
using notes.application.Constants;
using notes.application.Exceptions;

namespace notes.application.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> PasswordValidationRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(16)
                .Matches(AppConstants.PasswordRegex)
                .WithErrorCode(ErrorCodes.PasswordFormatIsNotValid);
        }

        public static IRuleBuilderOptions<T, string> EmailValidationRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                    .NotEmpty()
                    .NotNull()
                    .EmailAddress()
                    .WithErrorCode(ErrorCodes.EmailFormatIsNotValid);
        }

        public static IRuleBuilderOptions<T, string> StringValidationRule<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength, int maxLength)
        {
            return ruleBuilder
                .NotEmpty()
                .NotNull()
                .MinimumLength(minLength)
                .MaximumLength(maxLength);
        }

        public static IRuleBuilderOptions<T, string[]> TagsValidationRule<T>(this IRuleBuilder<T, string[]> ruleBuilder)
        {
            return (IRuleBuilderOptions<T, string[]>)ruleBuilder
                .NotNull()
                .Custom((tags, context) =>
            {
                if (tags != null)
                {
                    if (tags.Length > 5)
                    {
                        context.AddFailure(
                            new ValidationFailure("Tags", "")
                            {
                                ErrorCode = ErrorCodes.TagsCountIsLarge
                            });
                    }
                }
            });
        }

        public static IRuleBuilderOptions<T, string> NameValidationRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.StringValidationRule(3, 20);
        }

        public static IRuleBuilderOptions<T, string> TextValidationRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.StringValidationRule(10, 2000);
        }

        public static IRuleBuilderOptions<T, string> TitleValidationRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.StringValidationRule(5, 200);
        }
    }
}
