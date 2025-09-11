using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations.SearchServices
{
    public class SearchWord : ISearchService
    {
        public IEnumerable<NoteMetadata> SearchNotes(IEnumerable<NoteMetadata> notes, string searchTerm = null)
        {
            return notes; // not yet utilized
        }
    }
}
