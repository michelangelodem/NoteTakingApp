using System.Diagnostics.Metrics;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;
using NoteTakingApp.Configurations;
using NoteTakingApp.Services.Implementations;

namespace NoteTakingApp.Services.Commands
{
    public class CreateNote
    {
        private readonly NotesConfiguration _notesConfiguration;
        private static MetadataServices service;
        private INoteParser _parser;

        public CreateNote(NotesConfiguration notesConfiguration)
        {
            _notesConfiguration = notesConfiguration;
            service = new MetadataServices();
            _parser = new NoteParser();
        }

        public NoteMetadata CreateNoteCommand(string? initialContent = null)
        {
            var metadata = new NoteMetadata();
            try
            {
                metadata = _parser.Parse(initialContent);
            } 
            catch (ParsingNoteException ex)
            {
                throw new ParsingNoteException("Error creating note metadata", ex);
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata), "Metadata cannot be null");
            }

            return metadata;
        }
    }
}