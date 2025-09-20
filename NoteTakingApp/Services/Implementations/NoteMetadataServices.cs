using System.Collections;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Commands;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteMetadataServices : INoteService
    {   
        private readonly NotesConfiguration _notesConfiguration; 
        private Dictionary<string /*FileNameWithoutExtension*/, NoteMetadata /*note*/>? _noteCache;
        private INoteRepository _noteRepository;
        private readonly ILogger<NoteMetadataServices> _logger;

        public NoteMetadataServices(ILogger<NoteMetadataServices> logger, INoteRepository noteRepository)
        {
            _logger = logger;
            _notesConfiguration = new NotesConfiguration();
            _noteRepository = noteRepository;
            _noteCache = new Dictionary<string, NoteMetadata>();
        }

        public async Task<object> CreateNoteAsync(object? initialContent = null)
        {
            var createNote = new CreateNote(_notesConfiguration);
            try
            {
                var noteMetadata = createNote.CreateNoteCommand((string)initialContent);
                await _noteRepository.AddNoteFileAsync(noteMetadata);
                
                _noteCache[noteMetadata.FileNameWithoutExtension] = noteMetadata;
                return noteMetadata;
            }
            catch (Exception ex) when (ex is ParsingNoteException || ex is IOException)
            {
                _logger.LogError($"Error creating note {ex.Message}");
                throw new Exception("Error creating note", ex);
            }
        }

        public async Task<object> EditNoteAsync(object fileName, object content)
        {      
            var updateNote = new UpdateNote(new NotesConfiguration());
            var newNote = new NoteMetadata();
            try
            {
                var oldNote = await _noteRepository.GetNoteFileAsync((string)fileName);
                //Console.WriteLine($"{oldNote.Content}");

                newNote = await updateNote.UpdateNoteAsync(oldNote, (string)content);
                //Console.WriteLine($"{newNote.Content}");

                await _noteRepository.DeleteNoteFileAsync(oldNote.FileName);
                await _noteRepository.AddNoteFileAsync(newNote);
            }
            catch (Exception ex) when (ex is ParsingNoteException || ex is IOException)
            {
                _logger.LogError($"NoteService:Error updating file {fileName} :{ex.Message}");
                throw new Exception("Error updating note", ex);
            }

            _noteCache[newNote.FileNameWithoutExtension] = newNote;

            return newNote;
        }

        public async Task<IEnumerable> LoadNoteAsync(object? filename = null)
        {
            IEnumerable<NoteMetadata> notes;
            string? fname = (string)filename;
            try {
                notes = await _noteRepository.GetAllNoteFilesAsync();
            }
            catch (Exception ex) when (ex is ParsingNoteException || ex is IOException)
            {
                _logger.LogError($"Error loading notes from repository: {ex.Message}");
                throw new Exception("Error loading notes from repository", ex);
            }

            _noteCache = notes.ToDictionary(
                    n => n.FileNameWithoutExtension,
                    n => n);
            
            if (!string.IsNullOrEmpty(fname))
            {
                return _noteCache[fname] != null 
                    ? new Dictionary<string, NoteMetadata> { { fname, _noteCache[fname] } } 
                    : new Dictionary<string, NoteMetadata>();
            }
            return _noteCache;
        }

        public async Task DeleteNoteAsync(object filePath)
        {
            try
            {
                await _noteRepository.DeleteNoteFileAsync((string)filePath);
                _noteCache.Remove(Path.GetFileNameWithoutExtension((string)filePath));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                _logger.LogError($"Error deleting note {filePath}: {ex.Message}");
                throw new Exception("Error deleting note", ex);
            }
        }
    }  
}
