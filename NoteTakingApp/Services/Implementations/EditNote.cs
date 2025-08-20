using NoteTakingApp.Configurations;
using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Implementations{
    public class EditNote 
    {
        private readonly NotesConfiguration _notesConfiguration; 
        private readonly Dictionary<string, NoteMetadata> _notes;
        private static MetadataServices service;
        
        public EditNote(NotesConfiguration notesConfiguration, Dictionary<string, NoteMetadata> notes) 
        {
            _notesConfiguration = notesConfiguration;
            _notes = notes;
            service = new MetadataServices();
        }

        public async Task<NoteMetadata?> EditNoteAsync(NoteMetadata oldNote, string newContent)
        {
            var oldTitle = oldNote.FileNameWithoutExtension;
            if (!_notes.ContainsKey(oldTitle))
            {
                Console.WriteLine($"Note with title '{oldTitle}' does not exist.");
                return null;
            }

            string? newTitle = service.ExtractTitleFromContent(newContent);
            string newFileName = oldNote.FileName;
            string newFileNameWithoutExtension = oldTitle;
            string newDisplayTitle = oldNote.DisplayTitle;

            if (!string.IsNullOrWhiteSpace(newTitle) && newTitle != oldNote.DisplayTitle)
            {
                newDisplayTitle = newTitle;
                newFileNameWithoutExtension = newTitle;
                newFileName = service.GenerateFileNameFromTitle(newTitle);

                var oldFilePath = Path.Combine(_notesConfiguration.NotesDirectory, oldNote.FileName);
                var newFilePath = Path.Combine(_notesConfiguration.NotesDirectory, newFileName);

                    
                if (File.Exists(oldFilePath))
                { 
                    File.Move(oldFilePath, newFilePath, overwrite: true);
                }

                
                _notes.Remove(oldTitle);
                oldNote.FileName = newFileName;
                oldNote.FileNameWithoutExtension = newFileNameWithoutExtension;
                oldNote.DisplayTitle = newDisplayTitle;
                _notes[newFileNameWithoutExtension] = oldNote;
            }
            else 
            {
                var filePath = Path.Combine(_notesConfiguration.NotesDirectory, oldNote.FileName);
                await File.WriteAllTextAsync(filePath, newContent);
            }

            oldNote.Content = newContent;
            oldNote.WordCount = service.CountWords(newContent);
            oldNote.UpdateLastModifiedDate();

            Console.WriteLine($"Note with title '{oldNote.DisplayTitle}' has been updated.");
            return oldNote;
        }
    }
}


