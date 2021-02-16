using FluentValidation;
using notes.api.Models;
using notes.application.Extensions;

namespace notes.api.Validators
{
    public class NotesRequestModelValidator<T> : AbstractValidator<NotesRequestModel>
    {
        public NotesRequestModelValidator()
        {

            RuleFor(x => x.Text)
                .TextValidationRule();

            RuleFor(x => x.Title)
                .TitleValidationRule();

            RuleFor(x => x.IsAscSort)
                .TitleValidationRule();

            RuleForEach(t => t.Tags)
                .NotNull()
                .NotEmpty()
                .StringValidationRule(5, 20);

            RuleFor(x => x.Tags)
                .TagsValidationRule();
        }
    }
}
