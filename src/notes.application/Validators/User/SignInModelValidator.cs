using FluentValidation;
using notes.application.Extensions;
using notes.application.Models.User;

namespace notes.application.Validators.User
{
    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailValidationRule();

            RuleFor(x => x.Password).PasswordValidationRule();
        }
    }
}
