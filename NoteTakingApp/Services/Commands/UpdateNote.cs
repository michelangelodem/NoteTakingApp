using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Implementations;
using NuGet.Protocol;

namespace NoteTakingApp.Services.Commands{
    public class UpdateNote 
    {
        private readonly NotesConfiguration _notesConfiguration; 
        private NoteParser _noteParser;
        
        public UpdateNote(NotesConfiguration notesConfiguration) 
        {
            _notesConfiguration = notesConfiguration;
            _noteParser = new NoteParser();
        }

        public async Task<NoteMetadata?> UpdateNoteAsync(NoteMetadata oldNote, string newContent)
        {
            var newNote = new NoteMetadata();
            try
            {
                newNote = _noteParser.Parse(newContent);
            } catch (ParsingNoteException ex)
            {
                throw new ParsingNoteException("Error parsing note metadata on the command layer", ex);
            }

            //The only time we need to update the note is when the content or title has changed
            //The only time we need to create a new note is when the title has changed
            //Any other change is an update to the existing note

            if (!newNote.FileName.Equals(oldNote.FileName)) 
            {
                return newNote;
            }

            if (!newNote.Content.Equals(oldNote.Content) && newNote.FileName.Equals(oldNote.FileName))
            {
                oldNote.Content = newNote.Content;
                oldNote.WordCount = newNote.WordCount;
                oldNote.UpdateLastModifiedDate();
            }

            return oldNote;
        }
    }
}


