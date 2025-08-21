using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteRepository : INoteRepository
    {
        private NotesConfiguration _notesConfiguration;
        private INoteParser _noteParser;
        
        public NoteRepository(NotesConfiguration notesConfiguration)
        {
            _notesConfiguration = notesConfiguration;
            _noteParser = new NoteParser();
        }

        public async Task<NoteMetadata> GetNoteFileAsync(string fileName)
        {
            var n_metadata = new NoteMetadata();

            var content = await File.ReadAllTextAsync(fileName);
            n_metadata = _noteParser.Parse(content);
            
            return n_metadata;
        }

        public async Task<IEnumerable<NoteMetadata>> GetAllNoteFilesAsync()
        {
            var filepath = _notesConfiguration.NotesDirectory;
            var notes = new List<NoteMetadata>();
            var files = Directory.GetFiles(filepath, "*.md");

            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                var metadata = _noteParser.Parse(content);
                notes.Add(metadata);
            }

            return notes.AsEnumerable();
        }

        public async Task AddNoteFileAsync(NoteMetadata note)
        {
            await Task.Run(async () =>
            {
                await File.WriteAllTextAsync(note.FileName, note.Content);
            });
        }

        public async Task DeleteNoteFileAsync(string fileName)
        {
            await Task.Run(() =>
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                } 
                else
                {
                    throw new FileNotFoundException($"The file {fileName} does not exist.");
                }
            });
        }

        public async Task UpdateNoteFileAsync(NoteMetadata note, NoteMetadata oldNote)
        {
            if (note.DisplayTitle != oldNote.DisplayTitle)
            {
                await Task.Run(async () =>
                {
                    await DeleteNoteFileAsync(oldNote.FileName);
                    await File.WriteAllTextAsync(note.FileName, note.Content);
                });
            }

            if (!note.Content.Equals(oldNote.Content) && note.DisplayTitle.Equals(oldNote.DisplayTitle))
            {
                oldNote.Content = note.Content;
                oldNote.WordCount = note.WordCount; 
                oldNote.UpdateLastModifiedDate();
                await File.WriteAllTextAsync(oldNote.FileName, oldNote.Content);
            }

            else throw new Exception("No changes detected in the note content or title.");
        }
    }
}
