using System.Diagnostics;
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
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, fileName);
            var content = await File.ReadAllTextAsync(filePath);
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
            string dir = _notesConfiguration.NotesDirectory;

            await Task.Run(async () =>
            {   
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string pathToDir = Path.Combine(dir, note.FileName);
                await File.WriteAllTextAsync(pathToDir, note.Content);
            });
        }

        public async Task DeleteNoteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, fileName);
            await Task.Run(() =>
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                } 
                else
                {
                    throw new FileNotFoundException($"The file {fileName} does not exist.");
                }
            });
        }

        public async Task UpdateNoteFileAsync(NoteMetadata note, NoteMetadata oldNote)
        {
            var oldFilePath = Path.Combine(_notesConfiguration.NotesDirectory, oldNote.FileName);
            var newFilePath = Path.Combine(_notesConfiguration.NotesDirectory, note.FileName);
           
            Console.WriteLine($"new note:" +
                              $"filename: {note.FileName}" +
                              $"content: {note.Content}" +
                              $"path: {newFilePath}");

            Console.WriteLine($"old note:" +
                              $"filename: {oldNote.FileName}" +
                              $"content: {oldNote.Content}" +
                              $"path: {oldFilePath}");

            if (note.DisplayTitle != oldNote.DisplayTitle)
            {
                await Task.Run(async () =>
                {
                    await DeleteNoteFileAsync(oldNote.FileName);
                    await File.WriteAllTextAsync(_notesConfiguration.NotesDirectory, note.Content);
                });
            }

            if (!note.Content.Equals(oldNote.Content) && note.DisplayTitle.Equals(oldNote.DisplayTitle))
            {
                oldNote.Content = note.Content;
                oldNote.WordCount = note.WordCount; 
                oldNote.UpdateLastModifiedDate();
                await File.WriteAllTextAsync(oldFilePath, oldNote.Content);
            }

            else throw new Exception("No changes detected in the note content or title.");
        }
    }
}
