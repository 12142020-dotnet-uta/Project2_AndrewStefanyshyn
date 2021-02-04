using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.ViewModels
{
    public class SongViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Artist")]
        public string ArtistName { get; set; }
        [Required]
        [Display(Name = "Genre")]
        public string Genre { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        [Display(Name = "Number Of Plays")]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int NumberOfPlays { get; set; } = 0;
        [Display(Name = "Lyrics")]
        public string Lyrics { get; set; } = null;
        [Display(Name = "URL Path")]
        public string UrlPath { get; set; }
        [Display(Name = "Original")]
        public Boolean isOriginal { get; set; } = true;

        public IFormFile file { get; set; }
        public string JpgStringSong { get; set; } = null;
    }
}
