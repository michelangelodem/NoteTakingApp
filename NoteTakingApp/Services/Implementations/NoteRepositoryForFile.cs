using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;
using NuGet.ProjectModel;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteRepositoryForFile : INoteRepository
    {
        private NotesConfiguration _notesConfiguration;
        private INoteParser _noteParser;
        private ILogger<NoteRepositoryForFile> _logger;

        public NoteRepositoryForFile(NotesConfiguration notesConfiguration, ILogger<NoteRepositoryForFile> logger)
        {
            _notesConfiguration = notesConfiguration;
            _noteParser = new NoteMetadataParser();
            _logger = logger;
        }

        public async Task<NoteMetadata> GetNoteAsync(string fileName)
        {
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, fileName);

            try
            {
                var content = await File.ReadAllTextAsync(filePath);
                return _noteParser.Parse(content);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "File not found: {FileName} in {Directory}", fileName, _notesConfiguration.NotesDirectory);
                throw new FileNotFoundException($"Note file '{fileName}' not found in '{_notesConfiguration.NotesDirectory}'.", ex);
            }
            catch (Exception ex) when (ex is FileLoadException || ex is ParsingNoteException)
            {
                _logger.LogError(ex, "Failed to load or parse note file: {FileName}", fileName);
                throw new Exception($"Failed to load or parse note file '{fileName}': {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<NoteMetadata>> GetAllNoteAsync()
        {
            var filepath = _notesConfiguration.NotesDirectory;
            var notes = new List<NoteMetadata>();
            var files = Directory.GetFiles(filepath, "*.md");

            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                var metadata = new NoteMetadata();

                try
                {
                    metadata = _noteParser.Parse(content);
                    notes.Add(metadata);
                }
                catch (ParsingNoteException ex)
                {
                    _logger.LogError(ex, "Error parsing note from file {FileName}", file);
                    throw new ParsingNoteException($"Error parsing note from file {file}: {ex.Message}", ex);
                }
            }

            return notes.AsEnumerable();
        }

        public async Task AddNoteAsync(NoteMetadata note)
        {
            string dir = _notesConfiguration.NotesDirectory;

            try
            {
                await Task.Run(async () =>
                {
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    string pathToDir = Path.Combine(dir, note.FileName);

                    await File.WriteAllTextAsync(pathToDir, note.Content);

                });
            }
            catch (DirectoryNotFoundException ex)
            {   
                _logger.LogError(ex, "Directory not found: {Directory}", dir);
                throw new DirectoryNotFoundException($"The directory {dir} was not found.", ex);
            }
            catch(Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
            {   
                _logger.LogError(ex, "Failed to add note file: {FileName}", note.FileName);
                throw new Exception($"Failed to add note file '{note.FileName} to Directory {dir}': {ex.Message}", ex);
            }
        }

        public async Task DeleteNoteAsync(string fileName)
        {
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, fileName);

            await Task.Run(() =>
            {
                try
                {
                    File.Delete(filePath);
                } 
                catch (FileNotFoundException e)
                {
                    _logger.LogError(e, "File not found: {FileName}", fileName);
                    throw new FileNotFoundException($"The file {fileName} does not exist: {e.Message}", e);
                }
            });
        }

        public async Task UpdateNoteContentAsync(string content, string fileName)
        {
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, fileName);
            await Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(filePath, content);
                }
                catch (FileNotFoundException e)
                {
                    _logger.LogError(e, "File not found: {FileName}", fileName);
                    throw new FileNotFoundException($"The file {fileName} does not exist: {e.Message}", e);
                }
            });
        }
    }
}
