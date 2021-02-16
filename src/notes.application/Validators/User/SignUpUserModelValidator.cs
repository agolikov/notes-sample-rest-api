using FluentValidation;
using notes.application.Extensions;
using notes.application.Models.User;
using notes.data.Interfaces;

namespace notes.application.Validators.User
{
    public class SignUpUserModelValidator : AbstractValidator<SignUpModel>
    {
        public SignUpUserModelValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .EmailValidationRule();

            RuleFor(x => x.Password).PasswordValidationRule();
        }
    }
}
