using NoteTakingApp.Configurations;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Commands;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteServices : INoteService
    {   
        private readonly NotesConfiguration _notesConfiguration; 
        private Dictionary<string /*FileNameWithoutExtension*/, NoteMetadata /*note*/>? _noteCache;
        private INoteRepository _noteRepository;

        public NoteServices()
        {
            _notesConfiguration = new NotesConfiguration();
            _noteRepository = new NoteRepository(_notesConfiguration);
            _noteCache = new Dictionary<string, NoteMetadata>();
        }

        public async Task<NoteMetadata> CreateNoteAsync(string? initialContent = null)
        {
            var createNote = new CreateNote(_notesConfiguration);
            try
            {
                var noteMetadata = createNote.CreateNoteCommand(initialContent);
                await _noteRepository.AddNoteFileAsync(noteMetadata);
                
                _noteCache[noteMetadata.FileNameWithoutExtension] = noteMetadata;
                return noteMetadata;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating note", ex);
            }
        }

        public async Task<NoteMetadata> EditNoteAsync(string fileName, string content)
        {      
            var updateNote = new UpdateNote(new NotesConfiguration());

            var oldNote = await _noteRepository.GetNoteFileAsync(fileName);
            Console.WriteLine($"{oldNote.Content}");

            var newNote = await updateNote.UpdateNoteAsync(oldNote, content);
            Console.WriteLine($"{newNote.Content}");
            
            await _noteRepository.DeleteNoteFileAsync(oldNote.FileName); 
            await _noteRepository.AddNoteFileAsync(newNote);
            _noteCache[newNote.FileNameWithoutExtension] = newNote;

            return newNote;
        }

        public async Task<Dictionary<string, NoteMetadata>> LoadNoteAsync()
        {
            var notes = await _noteRepository.GetAllNoteFilesAsync();
            _noteCache = notes.ToDictionary(
                n => n.FileNameWithoutExtension, 
                n => n
                );
            return _noteCache;
        }

        public async Task DeleteNoteAsync(string filePath)
        {
            try
            {
                await _noteRepository.DeleteNoteFileAsync(filePath);
                _noteCache.Remove(Path.GetFileNameWithoutExtension(filePath));
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting note", ex);
            }
        }
    }  
}
