using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class PlaylistLink
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int UserId { get; set; }
    }
}
