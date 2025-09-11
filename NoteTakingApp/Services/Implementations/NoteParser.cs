using Microsoft.VisualBasic;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteParser : INoteParser
    {
        private static readonly NotesConfiguration _notesConfiguration = new NotesConfiguration();
        private static IMetadataServices metadata_service = new MetadataServices();

        public NoteMetadata Parse(string contents)
        {       

            var metadata = metadata_service.SetNoteMetadata(contents);

            metadata.UpdateLastModifiedDate();
            return metadata;

        }
    }
}
