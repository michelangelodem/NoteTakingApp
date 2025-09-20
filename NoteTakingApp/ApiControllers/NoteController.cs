using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Context;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.ApiControllers
{
    [Route("api/[note_controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INoteService service, ILogger<NoteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Note), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        public async Task<ActionResult<Note>> AddNoteAsync(Note note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("Request received to create new note");
            var createdNote = (Note) (await _service.CreateNoteAsync(note));
            return CreatedAtAction(note.Metadata.DisplayTitle, new { id = createdNote.NoteId}, createdNote);
        }
    }
}
