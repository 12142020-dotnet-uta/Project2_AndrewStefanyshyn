using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class FavoriteList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int FavoriteListLink { get; set; }
        [Required]
        [Range(1,int.MaxValue, ErrorMessage ="Only positive numbers are allowed.")]
        public int SongId { get; set; }
        public int UserId { get; set; }

        public FavoriteList() { }
        public FavoriteList (int sId, int uId)
        {
            this.SongId = sId;
            this.UserId = uId;
        }
    }
    
}
