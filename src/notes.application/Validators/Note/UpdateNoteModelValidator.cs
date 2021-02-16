using FluentValidation;
using notes.application.Extensions;
using notes.application.Models.Note;

namespace notes.application.Validators.Note
{
    public class UpdateNoteModelValidator : AbstractValidator<UpdateNoteModel>
    {
        public UpdateNoteModelValidator()
        {
            RuleFor(x => x.Title).TitleValidationRule();

            RuleFor(x => x.Text).TextValidationRule();

            RuleFor(x => x.Tags).TagsValidationRule();
        }
    }
}
