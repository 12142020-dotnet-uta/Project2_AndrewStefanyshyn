using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Artist")]
        public string ArtistName { get; set; }
        [Required]
        [Display(Name="Genre")]
        public string Genre { get; set; }
        [Required]
        public string Title { get; set; }
        
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
        [NotMapped]
        public IFormFile file { get; set; }
        [NotMapped] public int year{ get; set;} = 0;
        public string JpgStringSong { get; set; } = null;
        public byte[] ByteArrayImage { get; set; } = null;
        public string albumUrl { get; set; }

        
        public Song(string artist, string genre, string title, string lyrics, string urlPath, bool isOriginal, string alubumurl)
        {
            this.ArtistName = artist;
            this.Genre = genre;
            this.Title = title;
            this.Lyrics = lyrics;
            this.UrlPath = urlPath;
            this.isOriginal = isOriginal;
            this.albumUrl = alubumurl;
        }

        public Song()
        {
        }
    }
}
