using FluentValidation;
using notes.application.Extensions;
using notes.application.Models.User;

namespace notes.application.Validators.User
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.NewPassword)
                .PasswordValidationRule();

            RuleFor(x => x.OldPassword).PasswordValidationRule();
        }
    }
}
