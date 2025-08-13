using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models
{
    public class NoteMetadata
    {

        public string FileName { get; set; } = string.Empty;
        public string FileNameWithoutExtension { get; set; } = string.Empty;
        public string DisplayTitle { get; set; } = string.Empty;
        public string ReferenceTitle { get; set; } = string.Empty;
        public bool HasH1Header { get; set; }
        public string Content { get; set; } = string.Empty;
        public int WordCount { get; set; } = 0;
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly LastModifiedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public void UpdateLastModifiedDate()
        {
            LastModifiedDate = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
