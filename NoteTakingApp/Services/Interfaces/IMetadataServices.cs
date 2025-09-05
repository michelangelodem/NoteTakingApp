using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface IMetadataServices
    {
        string GenerateFileNameFromTitle(string title);
        string EnsureUniqueFilePath(string originalPath);
        string FormatContent(string? initialContent, string displayTitle);
        string? ExtractTitleFromContent(string content);
        int CountWords(string content);
        string GetFilePathFromContent(string content);
        public (int, int) UpdateCount(string content);
        NoteMetadata SetNoteMetadata(string contents);
    }
}
