using NoteTakingApp.Configurations;
using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Implementations
{
    public class DeleteNote
    {
        private readonly NotesConfiguration _notesConfiguration;
        private readonly Dictionary<string /*title*/, NoteMetadata /*note*/> _notes;

        public DeleteNote(NotesConfiguration notesConfiguration, Dictionary<string, NoteMetadata> notes)
        {
            _notesConfiguration = notesConfiguration;
            _notes = notes;
        }

        public async Task DeleteNoteAsync(string title)
        {
            var noteExists = ValidateNoteExists(title);
            if (noteExists)
            {
                var filepath = Path.Combine(_notesConfiguration.NotesDirectory, _notes[title].FileName);
                Console.WriteLine($"Deleting note with title: {filepath} from directory: {_notesConfiguration.NotesDirectory}");
                File.Delete(filepath);
                _notes.Remove(title);
            }
            else
            {
                Console.WriteLine($"Note with title '{title}' does not exist.");
            }
            var files = Directory.GetFiles(_notesConfiguration.NotesDirectory, "*.md");
            foreach ( var file in files ) {
                Console.WriteLine($"File found: {file}");
            }
            Console.WriteLine($"Current notes count: {_notes.Count}");
            await Task.CompletedTask;

        }

        private bool ValidateNoteExists(string title)
        {
            if (!_notes.ContainsKey(title))
            {
                return false;
            }
            return true;
        }
    }
}
