using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models
{
    public class Note
    {
        public Guid Id { get; set; }  = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
