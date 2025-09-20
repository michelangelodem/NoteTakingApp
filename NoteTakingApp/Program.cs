using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Configurations;
using NoteTakingApp.Context;
using NoteTakingApp.Services.Implementations;
using NoteTakingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register your services
builder.Services.AddSingleton<NotesConfiguration>();
builder.Services.AddScoped<INoteService, NoteMetadataServices>();
builder.Services.AddScoped<INoteRepository, NoteRepositoryForFile>();
builder.Services.AddScoped<IMetadataServices, MetadataServices>();
builder.Services.AddScoped<INoteParser, NoteMetadataParser>();

// Configure DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
        ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();