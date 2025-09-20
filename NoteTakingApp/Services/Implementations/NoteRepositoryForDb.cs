using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NoteTakingApp.Configurations;
using NoteTakingApp.Context;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteRepositoryForDb : INoteRepository
    {
        private AppDbContext _context;
        private INoteParser _noteParser;
        private ILogger<NoteRepositoryForDb> _logger;

        public NoteRepositoryForDb(AppDbContext context, ILogger<NoteRepositoryForDb> logger)
        {
            _context = context;
            _noteParser = new NoteMetadataParser();
            _logger = logger;
        }

        public async Task<NoteMetadata> GetNoteFileAsync(string fileName)
        {

        }

        public async Task<IEnumerable<NoteMetadata>> GetAllNoteFilesAsync()
        {

        }

        public async Task AddNoteFileAsync(NoteMetadata note)
        {

        }

        public async Task DeleteNoteFileAsync(string fileName)
        {

        }

        public async Task UpdateNoteContentAsync(string content, string fileName)
        {

        }
    }
}

