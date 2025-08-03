using NoteTakingApp.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteTakingApp.Data;
using NoteTakingApp.Services.Implementations;
using NoteTakingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Database context
builder.Services.AddDbContextFactory<NoteTakingAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NoteTakingAppContext") ??
                         throw new InvalidOperationException("Connection string 'NoteTakingAppContext' not found.")));

// Services
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();