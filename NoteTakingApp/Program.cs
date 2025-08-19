using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using NoteTakingApp.Configurations;
using NoteTakingApp.Services.Implementations;
using NoteTakingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for Notes
builder.Services.Configure<NotesConfiguration>(
    builder.Configuration.GetSection("Notes"));

// Register INoteService with CreateNote implementation
builder.Services.AddSingleton<CreateNote>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    // For demo/testing, pass an empty dictionary
    return new CreateNote(config, new Dictionary<string, NoteTakingApp.Models.NoteMetadata>());
});
builder.Services.AddSingleton<LoadNotes>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    return new LoadNotes(config, new Dictionary<string, NoteTakingApp.Models.NoteMetadata>());
});
builder.Services.AddSingleton<DeleteNote>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    return new DeleteNote(config, new Dictionary<string, NoteTakingApp.Models.NoteMetadata>());
});

// Add Razor Pages and Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();