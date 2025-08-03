using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Data;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations 
{
    public class NoteService : INoteService
    {
        private readonly IDbContextFactory<NoteTakingAppContext> _dbContextFactory;

        public NoteService(IDbContextFactory<NoteTakingAppContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Note
                .OrderByDescending(n => n.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(Guid id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Note.FindAsync(id);
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            using var context = _dbContextFactory.CreateDbContext();
            note.Id = Guid.NewGuid();
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            context.Note.Add(note);
            await context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            using var context = _dbContextFactory.CreateDbContext();
            note.UpdatedAt = DateTime.UtcNow;

            context.Note.Update(note);
            await context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var note = await context.Note.FindAsync(id);
            if (note == null) return false;

            context.Note.Remove(note);
            await context.SaveChangesAsync();
            return true;
        }
    }
}