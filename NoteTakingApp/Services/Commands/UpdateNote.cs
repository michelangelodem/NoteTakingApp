using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Implementations;

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
            var newNote = _noteParser.Parse(newContent);

            //The only time we need to update the note is when the content or title has changed
            //The only time we need to create a new note is when the title has changed
            //Any other change is an update to the existing note

            if (newNote.DisplayTitle != oldNote.DisplayTitle) 
            {
                return newNote;
            }

            if (!newNote.Content.Equals(oldNote.Content) && newNote.DisplayTitle.Equals(oldNote.DisplayTitle))
            {
                oldNote.Content = newNote.Content;
                oldNote.WordCount = newNote.WordCount;
                oldNote.UpdateLastModifiedDate();
                return oldNote;
            }

            return oldNote;
        }
    }
}


