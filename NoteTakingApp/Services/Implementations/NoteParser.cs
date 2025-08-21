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

            var metadata = service.SetNoteMetadata(contents);

            metadata.UpdateLastModifiedDate();
            return metadata;

        }
    }
}
