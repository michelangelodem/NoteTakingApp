using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models
{
    public class User
    {
        [Key]
        public Int32 Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
