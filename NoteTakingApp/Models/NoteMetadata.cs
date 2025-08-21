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

        public override bool Equals(object obj)
        {
            if (obj is NoteMetadata other)
            {
                return FileName == other.FileName &&
                       FileNameWithoutExtension == other.FileNameWithoutExtension &&
                       DisplayTitle == other.DisplayTitle &&
                       ReferenceTitle == other.ReferenceTitle &&
                       HasH1Header == other.HasH1Header &&
                       Content == other.Content &&
                       WordCount == other.WordCount &&
                       CreatedDate == other.CreatedDate &&
                       LastModifiedDate == other.LastModifiedDate;
            }
            return false;
        }
    }
}
