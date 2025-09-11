using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface ISearchService
    {
        IEnumerable<NoteMetadata> SearchNotes(IEnumerable<NoteMetadata> notes, string searchTerm);

    }
}
