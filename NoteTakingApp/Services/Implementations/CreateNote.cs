using System.Diagnostics.Metrics;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;
using NoteTakingApp.Configurations;
using NuGet.Packaging;

namespace NoteTakingApp.Services.Implementations
{
    public class CreateNote
    {
        private readonly NotesConfiguration _notesConfiguration;
        private readonly Dictionary<string, NoteMetadata> _notes;
        private static MetadataServices service;

        public CreateNote(NotesConfiguration notesConfiguration, Dictionary<string, NoteMetadata> notes)
        {
            _notesConfiguration = notesConfiguration;
            _notes = notes;
            service = new MetadataServices();
        }

        public async Task<NoteMetadata> CreateNoteAsync(string title, string? initialContent = null)
        {

            var fileName = service.GenerateFileNameFromTitle(title);
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, $"{fileName}.md");

            filePath = service.EnsureUniqueFilePath(filePath, _notes.Count);
            fileName = Path.GetFileNameWithoutExtension(filePath);

            if (string.IsNullOrWhiteSpace(title))
            {
                title = fileName;
            }

            var content = service.FormatContent(initialContent, title);

            await File.WriteAllTextAsync(filePath, content);
            var metadata = NoteParser.ParseNote(filePath, content);
            return metadata;
        }
    }
}
