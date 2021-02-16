using FluentValidation;
using notes.application.Extensions;
using notes.application.Models.Note;

namespace notes.application.Validators.Note
{
    public class NoteModelValidator : AbstractValidator<NoteModel>
    {
        public NoteModelValidator()
        {
            RuleFor(x => x.Text).TextValidationRule();

            RuleFor(x => x.Title).TitleValidationRule();

            RuleFor(x => x.Tags).TagsValidationRule();

            RuleForEach(t => t.Tags)
                .NotNull()
                .NotEmpty()
                .NameValidationRule();
        }
    }
}
