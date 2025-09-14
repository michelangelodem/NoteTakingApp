using System.Diagnostics;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;
using NuGet.ProjectModel;

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
            string content;
            try
            {
                content = await File.ReadAllTextAsync(filePath);
                n_metadata = _noteParser.Parse(content);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException($"The file {fileName} does not exist: {e.Message}", e);
            }
            catch (FileLoadException e)
            {
                throw new FileLoadException($"The file {fileName} could not be loaded: {e.Message}", e);
            }
            catch (ParsingNoteException e)
            {
                throw new ParsingNoteException($"Error parsing note metadata for file {fileName}: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred while reading the file {fileName}: {e.Message}", e);
            } 

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
                var metadata = new NoteMetadata();

                try
                {
                    metadata = _noteParser.Parse(content);
                    notes.Add(metadata);
                }
                catch (EndOfStreamException e)
                {
                    throw new EndOfStreamException($"Error parsing file {file}: {e.Message}");
                }
                catch (FileFormatException e)
                {
                    throw new FileFormatException($"Error parsing file {file}: {e.Message}");
                }
                catch (ParsingNoteException e)
                {
                    throw new ParsingNoteException($"Error parsing file {file}: {e.Message}");
                }
            }

            return notes.AsEnumerable();
        }

        public async Task AddNoteFileAsync(NoteMetadata note)
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
                throw new DirectoryNotFoundException($"The directory {dir} was not found.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"You do not have permission to access the directory {dir}.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new PathTooLongException($"The path {dir} is too long.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"An I/O error occurred while accessing the directory {dir}.", ex);
            }
        }

        public async Task DeleteNoteFileAsync(string fileName)
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
                    throw new FileNotFoundException($"The file {fileName} does not exist: {e.Message}", e);
                }
            });
        }
    }
}
