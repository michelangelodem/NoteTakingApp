using System.Collections;
using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<object> CreateNoteAsync(object? initialContent = null);
        Task<object> EditNoteAsync(object fileName, object content);
        Task<IEnumerable> LoadNoteAsync(object filename = null);
        Task DeleteNoteAsync(object filePath);
    }
}
