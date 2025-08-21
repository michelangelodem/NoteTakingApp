using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteMetadata> CreateNoteAsync(string? initialContent = null);
        Task<NoteMetadata> EditNoteAsync(string fileName, string content);
        Task<Dictionary<string, NoteMetadata>> LoadNoteAsync(string filePath);
        Task DeleteNoteAsync(string filePath);
    }
}
