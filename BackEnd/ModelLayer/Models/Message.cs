using System;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int ToUserId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime dateTime { get; set; } = DateTime.Now;

        public Message() { }
        public Message(string FromUserName, int toUser, int fromUser, string content) 
        {
            this.ToUserId = toUser;
            this.FromUserId = fromUser;
            this.Content = content;
            this.FromUserName = FromUserName;
        }
    }
}
