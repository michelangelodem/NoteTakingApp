using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteMetadata> CreateNoteAsync(string title, string? initialContent = null);
    }
}
