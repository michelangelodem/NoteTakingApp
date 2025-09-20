using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteTakingApp.Configurations;
using NoteTakingApp.Context;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Commands;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    //public class DbNoteService : INoteService
    //{
    //    private AppDbContext _context;
    //    private NoteMetadataServices _service;

    //    public DbNoteService(AppDbContext context, NoteMetadataServices nm_service)
    //    {
    //        _context = context;
    //        _service = nm_service;
    //    }

    //    public async Task<object> CreateNoteAsync(object? initialContent = null)
    //    {

    //    }

    //    public async Task<object> EditNoteAsync(object fileName, object content)
    //    {

    //    }

    //    public async Task<IEnumerable> LoadNoteAsync(object? filename = null)
    //    {

    //    }

    //    public async Task DeleteNoteAsync(string filePath)
    //    {

    //    }

    //}
}
