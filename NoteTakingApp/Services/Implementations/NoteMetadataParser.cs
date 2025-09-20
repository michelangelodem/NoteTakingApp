using Microsoft.VisualBasic;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteMetadataParser : INoteParser
    {
        private static readonly NotesConfiguration _notesConfiguration = new NotesConfiguration();
        private static IMetadataServices metadata_service = new MetadataServices();

        public object Parse(string contents)
        {
            var metadata = new NoteMetadata();

            metadata = metadata_service.SetNoteMetadata(contents);

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
