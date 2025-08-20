using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteParser
    {
        private static readonly NotesConfiguration _notesConfiguration = new NotesConfiguration();
        private static MetadataServices service = new MetadataServices();

        public static NoteMetadata ParseNote(string filePath, string contents)
        {
            var fileName = Path.GetFileName(filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var wordCount = service.CountWords(contents);
            var content = contents;

            var h1Header = service.ExtractTitleFromContent(contents);
            var hasH1Header = !(string.IsNullOrEmpty(h1Header));
            var displayTitle = hasH1Header 
                ? h1Header
                : fileNameWithoutExtension;

            var metadata = new NoteMetadata
            {
                FileName = fileName,
                FileNameWithoutExtension = fileNameWithoutExtension,
                DisplayTitle = displayTitle ?? fileNameWithoutExtension,
                ReferenceTitle = fileNameWithoutExtension, // Default to file name without extension
                Content = content,
                WordCount = wordCount,
                LastModifiedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            metadata.UpdateLastModifiedDate();
            return metadata;

        }
    }
}
