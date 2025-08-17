using System.Diagnostics.Metrics;
using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;
using NoteTakingApp.Configurations;
using NuGet.Packaging;

namespace NoteTakingApp.Services.Implementations
{
    public class CreateNote
    {
        private readonly NotesConfiguration _notesConfiguration;
        private readonly Dictionary<string, NoteMetadata> _notes;

        public CreateNote(NotesConfiguration notesConfiguration, Dictionary<string, NoteMetadata> notes)
        {
            _notesConfiguration = notesConfiguration;
            _notes = notes;
        }

        public async Task<NoteMetadata> CreateNoteAsync(string title, string? initialContent = null)
        {

            var fileName = GenerateFileNameFromTitle(title);
            var filePath = Path.Combine(_notesConfiguration.NotesDirectory, $"{fileName}.md");

            filePath = EnsureUniqueFilePath(filePath);
            fileName = Path.GetFileNameWithoutExtension(filePath);

            if(string.IsNullOrWhiteSpace(title))
            {
                title = fileName;
            }

            var content = SetContentAccordingToInitialContent(initialContent, title);

            await File.WriteAllTextAsync(filePath, content);
            var metadata = NoteParser.ParseNote(filePath, content);
            return metadata;
        }

        private string GenerateFileNameFromTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = "Untitled_Note";
            }
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string fileName = title.Trim();

            fileName = ReplaceInvalidCharsWithUnderscore(invalidChars, fileName);

            if (fileName.Length > 100)
            {
                fileName = fileName.Substring(0, 100);
            }
            return fileName;
        }

        private string ReplaceInvalidCharsWithUnderscore(char[] invalidChars, string inputString)
        {
            foreach (char c in invalidChars)
            {
                inputString = inputString.Replace(c, '_');
            }

            string outputString = inputString
                .Replace(' ', '_')
                .Replace('/', '_')
                .Replace('\\', '_')
                .Replace(':', '_')
                .Replace('*', '_')
                .Replace('?', '_')
                .Replace('"', '_')
                .Replace('<', '_')
                .Replace('>', '_')
                .Replace('|', '_');
        
            return outputString;
        }

        private string EnsureUniqueFilePath(string originalPath)
        {
            if (!File.Exists(originalPath))
                return originalPath;

            var directory = Path.GetDirectoryName(originalPath);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalPath);
            var extension = Path.GetExtension(originalPath);
            
            int counter = 1;
            string newPath = originalPath;
            while (counter < _notes.Count)
            {
                var newFileName = $"{fileNameWithoutExt}_{counter}";
                newPath = Path.Combine(directory!, $"{newFileName}{extension}");
                counter++;
            }

            return newPath;
        }

        private string SetContentAccordingToInitialContent(string? initialContent, string defaultMessage)
        {
            if (string.IsNullOrEmpty(initialContent))
            {
                return $"# {defaultMessage} \n\n";
            }
            else if (!initialContent.TrimStart().StartsWith("# "))
            {
                return $"# {defaultMessage} \n\n{initialContent}";
            }
            return initialContent;
        }
    }

}
