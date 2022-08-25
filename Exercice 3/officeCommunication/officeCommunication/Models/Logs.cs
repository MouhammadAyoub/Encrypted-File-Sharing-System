using System.ComponentModel.DataAnnotations;

namespace officeCommunication.Models
{
    record UserDto(string UserName, string Password);
    public class Logs
    {
        [Key]
        public int log_id { get; set; }

        [Required]
        public DateTime datetime { get; set; }

        [Required]
        public bool action_type { get; set; }

        [Required]
        public string? filename { get; set; }

    }
}
