
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteService : INoteService
    {

        private readonly List<Note> _notes = new();
        private Guid _nextId = Guid.NewGuid();

        public async Task<List<Note>> GetAllNotesAsync()
        {
            var notes = _notes.OrderByDescending(n => n.UpdatedAt);
            return await Task.FromResult(notes.ToList());
        }

        public async Task<Note?> GetNoteByIdAsync(Guid id)
        {
            var note = _notes.FirstOrDefault(n => n.Id.Equals(id));
            return await Task.FromResult(note);
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            note.Id = _nextId;
            _notes.Add(note);
            return await Task.FromResult(note);
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            var existingNote = _notes.FirstOrDefault(n => n.Id.Equals(note.Id));
            if (existingNote != null)
            {
                existingNote.Title = note.Title;
                existingNote.Content = note.Content;
                existingNote.UpdatedAt = DateTime.UtcNow;
            }

            return await Task.FromResult(existingNote);
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            var note = _notes.FirstOrDefault(n => n.Id.Equals(0));
            if (note != null)
            {
                _notes.Remove(note);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<List<Note>> SearchNotesAsync(String searchTerm)
        {
            var searchResult =
                _notes.Where(n => n.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));
            return await Task.FromResult(searchResult.ToList());
        }
    }
};
