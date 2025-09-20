using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models
{
    public class Room
    {
        [Key]
        public Int32 RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public bool IsPrivate { get; set; }
        public Int32 NoteCount { get; set; }
        public Int32 AdminId { get; set; }
        public Int32 ContributorCount { get; set; }
    }
}
