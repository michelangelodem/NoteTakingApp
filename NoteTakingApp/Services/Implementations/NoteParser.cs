using Microsoft.VisualBasic;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteParser : INoteParser
    {
        private static readonly NotesConfiguration _notesConfiguration = new NotesConfiguration();
        private static IMetadataServices service = new MetadataServices();
        private NoteMetadata _metadata;

        public NoteMetadata Parse(string contents)
        {       
            SetNoteMetadata(contents);

            var metadata = _metadata;

            metadata.UpdateLastModifiedDate();
            return metadata;

        }

        private void SetNoteMetadata(string contents)
        {
            _metadata = new NoteMetadata();

            var filePath = service.GetFilePathFromContent(contents);
            _metadata.FileName = Path.GetFileName(filePath);
            _metadata.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            _metadata.WordCount = service.CountWords(contents);
            _metadata.Content = contents;

            var h1Header = service.ExtractTitleFromContent(contents);
            _metadata.HasH1Header = !(string.IsNullOrEmpty(h1Header));
            _metadata.DisplayTitle = _metadata.HasH1Header
                ? h1Header
                : _metadata.FileNameWithoutExtension;
        }
    }
}
