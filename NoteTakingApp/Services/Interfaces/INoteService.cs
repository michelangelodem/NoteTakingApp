using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetAllNotesAsync();
        Task<Note?> GetNoteByIdAsync(Guid id);
        Task<Note> CreateNoteAsync(Note note);
        Task<Note> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(Guid id);
        Task<List<Note>> SearchNotesAsync(string searchTerm);
    }
}
