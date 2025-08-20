namespace NoteTakingApp.Services.Interfaces
{
    public interface IMetadataServices
    {
        string GenerateFileNameFromTitle(string title);
        string EnsureUniqueFilePath(string originalPath, int fileQuantityInFolder);
        string FormatContent(string? initialContent, string displayTitle);
        string? ExtractTitleFromContent(string content);
        int CountWords(string content);
        string GetFilePathFromContent(string content);
    }
}
