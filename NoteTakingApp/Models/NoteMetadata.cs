namespace NoteTakingApp.Models
{
    public class NoteMetadata
    {

        public string FileName { get; set; } = string.Empty;
        public string FileNameWithoutExtension { get; set; } = string.Empty;
        public string DisplayTitle { get; set; } = string.Empty;
        public bool HasH1Header { get; set; }
        public string Content { get; set; } = string.Empty;
        public int WordCount { get; set; } = 0;
    }
}
