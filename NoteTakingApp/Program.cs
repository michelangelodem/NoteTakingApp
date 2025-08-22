using Microsoft.Extensions.Options;
using NoteTakingApp.Configurations;
using NoteTakingApp.Services.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for Notes
builder.Services.Configure<NotesConfiguration>(
    builder.Configuration.GetSection("Notes"));

// Register INoteService with CreateNote implementation
builder.Services.AddSingleton<CreateNote>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    // For demo/testing, pass an empty dictionary
    return new CreateNote(config);
});
builder.Services.AddSingleton<LoadNotes>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    return new LoadNotes();
});
builder.Services.AddSingleton<DeleteNote>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    return new DeleteNote();
});
builder.Services.AddSingleton<UpdateNote>(sp =>
{
    var config = sp.GetRequiredService<IOptions<NotesConfiguration>>().Value;
    return new UpdateNote(config);
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