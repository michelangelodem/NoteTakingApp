using NoteTakingApp.Configurations;
using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Implementations{
    public class EditNote 
    {
        private readonly NotesConfiguration _notesConfiguration; 
        private readonly Dictionary<string, NoteMetadata> _notes;
        
        public EditNote(NotesConfiguration notesConfiguration, Dictionary<string, NoteMetadata> notes) 
        {
            _notesConfiguration = notesConfiguration;
            _notes = notes;
        }

        public async Task<NoteMetadata?> EditNoteAsync(NoteMetadata oldNote, string newContent)
        {
            var oldTitle = oldNote.FileNameWithoutExtension;
            if (!_notes.ContainsKey(oldTitle))
            {
                Console.WriteLine($"Note with title '{oldTitle}' does not exist.");
                return null;
            }

            string? newTitle = ExtractTitleFromContent(newContent);
            string newFileName = oldNote.FileName;
            string newFileNameWithoutExtension = oldTitle;
            string newDisplayTitle = oldNote.DisplayTitle;

            if (!string.IsNullOrWhiteSpace(newTitle) && newTitle != oldNote.DisplayTitle)
            {
                newDisplayTitle = newTitle;
                newFileNameWithoutExtension = newTitle;
                newFileName = GenerateFileNameFromTitle(newTitle);

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
            oldNote.WordCount = CountWords(newContent);
            oldNote.UpdateLastModifiedDate();

            Console.WriteLine($"Note with title '{oldNote.DisplayTitle}' has been updated.");
            return oldNote;
        }

        private string? ExtractTitleFromContent(string content)
        {
            using var reader = new StringReader(content);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("# "))
                {
                    return line.Substring(2).Trim();
                }
            }
            return null;
        }

        private string GenerateFileNameFromTitle(string title)
        {
            var invalidChars = Path.GetInvalidFileNameChars(); 
            var safeTitle = string.Join("_", title.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).Trim(); 
            return $"{safeTitle}.md";
        }

        private int CountWords(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return 0; 
            var words = content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries); 
            return words.Length;
        }
    }
}


