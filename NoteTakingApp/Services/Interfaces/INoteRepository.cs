using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteRepository
    {
        public Task<NoteMetadata> GetNoteFileAsync(string fileName);
        public Task<IEnumerable<NoteMetadata>> GetAllNoteFilesAsync();
        public Task AddNoteFileAsync(NoteMetadata note);
        public Task DeleteNoteFileAsync(string fileName);
        public Task UpdateNoteFileAsync(NoteMetadata note, NoteMetadata oldNote);
    }
}
