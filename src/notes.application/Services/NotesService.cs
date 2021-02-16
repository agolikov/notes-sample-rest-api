using AutoMapper;
using notes.application.Interfaces;
using notes.application.Models.Common;
using notes.application.Models.Note;
using notes.data.Entities;
using notes.data.Exceptions;
using notes.data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace notes.application.Services
{
    public class NotesService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public NotesService(IMapper mapper,
            INoteRepository noteRepository,
            IUserRepository userRepository,
            ITagRepository tagRepository)
        {
            (_noteRepository, _mapper, _userRepository, _tagRepository) = (noteRepository, mapper, userRepository, tagRepository);
        }

        public async Task<PaginatedResult<NoteModel>> GetNotesAsync(NoteQueryModel model, Guid ownerId)
        {
            var notes = await _noteRepository.GetAsync(
                ownerId,
                model.Page,
                model.Size,
                model.IsAscSort,
                model.SortProperty,
                model.Title,
                model.Text,
                model.Tags
            );

            var count = await _noteRepository.CountAsync(
                ownerId,
                model.Title,
                model.Text,
                model.Tags);

            return new PaginatedResult<NoteModel>
            {
                Items = notes.Select(note => _mapper.Map<Note, NoteModel>(note)),
                TotalCount = count,
                Size = model.Size,
                Page = model.Page
            };
        }

        public async Task<NoteModel> CreateNoteAsync(CreateNoteModel model, Guid ownerId)
        {
            var user = await FindUserById(ownerId);

            var noteToAdd = _mapper.Map<Note>(model);

            noteToAdd.Owner = user;

            noteToAdd.Tags = (await _tagRepository.InsertTagsAsync(model.Tags, ownerId)).ToList();

            var noteAdded = await _noteRepository.InsertOrUpdateAsync(noteToAdd, ownerId);

            return _mapper.Map<Note, NoteModel>(noteAdded);
        }

        public async Task<NoteModel> UpdateNoteAsync(UpdateNoteModel model, Guid ownerId)
        {
            var noteEntity = await _noteRepository.FindOneAsync(t => t.Id == model.Id);
            if (noteEntity == null)
            {
                throw model.Id.EntityNotFoundException();
            }

            if (noteEntity.CreatedBy != ownerId)
            {
                throw model.Id.EntityNotFoundException();
            }

            noteEntity.Tags = (await _tagRepository.InsertTagsAsync(model.Tags, ownerId)).ToList();
            noteEntity.Title = model.Title;
            noteEntity.Text = model.Text;

            var updatedNote = await _noteRepository.InsertOrUpdateAsync(noteEntity, noteEntity.CreatedBy);

            return _mapper.Map<Note, NoteModel>(updatedNote);
        }

        public async Task DeleteNoteAsync(Guid noteId, Guid ownerId)
        {
            var noteEntity = await _noteRepository.FindOneAsync(t => t.Id == noteId);
            if (noteEntity == null)
            {
                throw ownerId.EntityNotFoundException();
            }

            if (noteEntity.CreatedBy != ownerId)
            {
                throw ownerId.EntityNotFoundException();
            }

            await _noteRepository.DeleteAsync(noteId, ownerId);
        }

        private async Task<User> FindUserById(Guid userId)
        {
            var user = await _userRepository.FindOneAsync(t => t.Id == userId);
            if (user == null)
            {
                throw userId.EntityNotFoundException();
            }

            return user;
        }
    }
}
