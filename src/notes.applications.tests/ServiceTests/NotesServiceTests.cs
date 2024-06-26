﻿using System.Linq.Expressions;
using AutoFixture;
using Moq;
using notes.application.Interfaces;
using notes.application.Models.Note;
using notes.application.Services;
using notes.applications.tests.Common;
using notes.data.Entities;
using notes.data.Exceptions;
using notes.data.Interfaces;
using NUnit.Framework;
using ErrorCodes = notes.application.Exceptions.ErrorCodes;

namespace notes.applications.tests.ServiceTests
{
    [TestFixture(Category = "Unit")]
    public class NotesServiceTests : TestsBase
    {
        private INoteService _noteService;

        [SetUp]
        public void Setup()
        {
            _fixture = CreateFixture();
            _mapper = CreateMapper();
            
            _noteService = new NotesService(
                _mapper,
                NoteRepositoryMock.Object,
                UserRepositoryMock.Object,
                TagRepositoryMock.Object);
        }

        [Test]
        public void NotesService_CreateNote_User_Not_Found()
        {
            var note = _fixture.Create<CreateNoteModel>();
            Guid ownerId = Guid.NewGuid();

            var exception = Assert.ThrowsAsync<DalException>(async () => await _noteService.CreateNoteAsync(note, ownerId));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.EntityNotFound);
        }

        [Test]
        public async Task NotesService_CreateNote_OK()
        {
            var user = MockUser();
            var note = _fixture
                .Build<CreateNoteModel>()
                .Create();

            await _noteService.CreateNoteAsync(note, user.Id);

            NoteRepositoryMock.Verify(t => t.InsertOrUpdateAsync(It.Is<Note>(t => t.Text == note.Text), user.Id));
        }

        [Test]
        public void NotesService_UpdateNote_Note_Not_Found()
        {
            var updateModel = _fixture.Create<UpdateNoteModel>();
            Guid ownerId = Guid.NewGuid();

            var exception = Assert.ThrowsAsync<DalException>(async () => await _noteService.UpdateNoteAsync(updateModel, ownerId));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.EntityNotFound);
        }

        [Test]
        public async Task NotesService_UpdateNote_OK()
        {
            var user = MockUser();
            var note = _fixture.Build<Note>()
                .With(n => n.Owner, user)
                .With(t => t.CreatedBy, user.Id)
                .Create();

            NoteRepositoryMock
                .Setup(t => t.FindOneAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(note);

            var updateModel = _fixture
                .Build<UpdateNoteModel>()
                .With(t => t.Id, note.Id)
                .Create();

            await _noteService.UpdateNoteAsync(updateModel, user.Id);

            //Assert
            NoteRepositoryMock.Verify(t => t.InsertOrUpdateAsync(It.IsAny<Note>(), user.Id), Times.Once);
        }

        [Test]
        public async Task NotesService_DeleteNote_OK()
        {
            var user = MockUser();
            var note = _fixture.Build<Note>()
                .With(n => n.Owner, user)
                .With(t => t.CreatedBy, user.Id)
                .Create();

            NoteRepositoryMock
                .Setup(t => t.FindOneAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(note);

            await _noteService.DeleteNoteAsync(note.Id, user.Id);

            NoteRepositoryMock.Verify(t => t.DeleteAsync(note.Id, user.Id, false));
        }

        [Test]
        public void NotesService_DeleteNote_Note_Not_Found()
        {
            Guid ownerId = Guid.NewGuid();
            Guid noteId = Guid.NewGuid();

            var exception = Assert.ThrowsAsync<DalException>(async () => await _noteService.DeleteNoteAsync(noteId, ownerId));

            //Assert
            Assert.AreEqual(exception.Code, ErrorCodes.EntityNotFound);
        }
    }
}