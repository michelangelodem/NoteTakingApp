using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models
{
    public class Note
    {
        [Key]
        public Int32 NoteId { get; set; } = 0;
        public NoteMetadata Metadata { get; set; } = new ();
        [Required]
        public Int32 RoomId { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
