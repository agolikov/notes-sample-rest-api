using notes.application.Models.Common;
using notes.application.Models.Note;
using System;
using System.Threading.Tasks;

namespace notes.application.Interfaces
{
    public interface INoteService
    {
        Task<PaginatedResult<NoteModel>> GetNotesAsync(NoteQueryModel model, Guid ownerId);

        Task<NoteModel> CreateNoteAsync(CreateNoteModel model, Guid ownerId);

        Task<NoteModel> UpdateNoteAsync(UpdateNoteModel model, Guid ownerId);

        Task DeleteNoteAsync(Guid noteId, Guid ownerId);
    }
}
