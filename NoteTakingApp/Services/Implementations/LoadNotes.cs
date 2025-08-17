using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.PointsToAnalysis;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class LoadNotes
    {
        private readonly NotesConfiguration _notesConfiguration;
        private readonly Dictionary<string /*title*/, NoteMetadata /*note*/ >? _notesCache;

        public LoadNotes(NotesConfiguration notesConfiguration, Dictionary<string, NoteMetadata> notesCache)
        {
            _notesConfiguration = notesConfiguration;
            _notesCache = notesCache;
        }

        public async Task LoadNotesAsync()
        {
            string? filepath = _notesConfiguration.NotesDirectory;

            var files = Directory.GetFiles(filepath, "*.md");
            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                var metadata = NoteParser.ParseNote(file, content);
                if (metadata != null)
                {
                    _notesCache[metadata.FileNameWithoutExtension] = metadata;
                }
            }
        }
    }
}
