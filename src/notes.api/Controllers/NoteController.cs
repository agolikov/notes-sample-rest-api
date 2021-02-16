using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using notes.api.Extensions;
using notes.api.Models;
using notes.application.Interfaces;
using notes.application.Models.Common;
using notes.application.Models.Note;
using System;
using System.Threading.Tasks;

namespace notes.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notes")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IMapper _mapper;

        public NoteController(INoteService noteService, IMapper mapper)
        {
            _noteService = noteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<NoteModel>>> GetNotesAsync([FromQuery] NotesRequestModel model)
        {
            var userId = HttpContext.GetSessionUserId();

            var queryModel = _mapper.Map<NoteQueryModel>(model);

            var notes = await _noteService.GetNotesAsync(
                queryModel, userId);

            return Ok(notes);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateNoteModel model)
        {
            var ownerId = HttpContext.GetSessionUserId();

            var note = await _noteService.CreateNoteAsync(model, ownerId);

            return Ok(note);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateNoteModel model)
        {
            var ownerId = HttpContext.GetSessionUserId();

            var note = await _noteService.UpdateNoteAsync(model, ownerId);

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var ownerId = HttpContext.GetSessionUserId();

            await _noteService.DeleteNoteAsync(id, ownerId);

            return NoContent();
        }
    }
}