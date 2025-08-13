using NoteTakingApp.Services.Interfaces;
using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteParser
    {
        public static NoteMetadata ParseNote(string filePath, string contents)
        {
            var fileName = Path.GetFileName(filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var wordCount = GetWordCount(contents);
            var content = contents;

            var h1Header = GetFirstH1Header(content);
            var hasH1Header = !(string.IsNullOrEmpty(h1Header));
            var displayTitle = hasH1Header 
                ? h1Header
                : fileNameWithoutExtension;

            var metadata = new NoteMetadata
            {
                FileName = fileName,
                FileNameWithoutExtension = fileNameWithoutExtension,
                DisplayTitle = displayTitle,
                ReferenceTitle = fileNameWithoutExtension, // Default to file name without extension
                Content = content,
                WordCount = wordCount,
                LastModifiedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            metadata.UpdateLastModifiedDate();
            return metadata;

            }

        private static string? GetFirstH1Header(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            else  
            {
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    string trimmedLine = line.Trim();

                    if (trimmedLine.Length == 0) continue;

                    if (trimmedLine.StartsWith("# ") && trimmedLine.Length > 2)
                    {
                        return trimmedLine.Substring(2).Trim(); // Remove the "# " prefix
                    }
                    else break;
                }
            }
            return null;
        }

        private static int GetWordCount(string content)
        {
            if (string.IsNullOrEmpty(content)) return 0;

            var words = content.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }
}
