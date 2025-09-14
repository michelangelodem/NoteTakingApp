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

            var metadata = new NoteMetadata();
            try
            {
                metadata = metadata_service.SetNoteMetadata(contents);
                metadata.UpdateLastModifiedDate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing note metadata on the parser layer", ex);
            }

            return metadata;
        }
    }

    public class ParsingNoteException : Exception
    {
        public ParsingNoteException() { }
        public ParsingNoteException(string message) 
            : base(message) { }
        public ParsingNoteException(string message, Exception inner) 
            : base(message, inner) { }
    }
}
