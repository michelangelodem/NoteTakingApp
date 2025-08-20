using NoteTakingApp.Components.Pages;

namespace NoteTakingApp.Services.Implementations
{
    public class MetadataServices
    {
        public MetadataServices () { }

        public string GenerateFileNameFromTitle(string title)
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

        public string EnsureUniqueFilePath(string originalPath, int count)
        {
            if (!File.Exists(originalPath))
                return originalPath;

            var directory = Path.GetDirectoryName(originalPath);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalPath);
            var extension = Path.GetExtension(originalPath);

            int counter = 1;
            string newPath = originalPath;
            while (counter < count)
            {
                var newFileName = $"{fileNameWithoutExt}_{counter}";
                newPath = Path.Combine(directory!, $"{newFileName}{extension}");
                counter++;
            }

            return newPath;
        }

        public string FormatContent(string? initialContent, string defaultMessage)
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

        public string? ExtractTitleFromContent(string content)
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
                        Console.WriteLine($"Parsing H1 Header: {trimmedLine}");
                        return trimmedLine.Substring(2).Trim(); // Remove the "# " prefix
                    }
                    else break;
                }
            }
            return null;
        }

        public int CountWords(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return 0;
            var words = content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }
}

