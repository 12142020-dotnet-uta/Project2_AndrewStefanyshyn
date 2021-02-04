using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class FriendList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int FriendListLink { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed.")]
        public int FriendId { get; set; }
        [Required]
        public int RequestedFriendId { get; set; }
        public string status { get; set; } = "pending";
        public string ToUsername { get; set; }
        public string FromUsername { get; set; }

        public FriendList() { }
        public FriendList(int userid, int requestedFriendid, string touserName,string fromuserName)
        {
            this.FriendId = userid;
            this.RequestedFriendId = requestedFriendid;
            this.ToUsername = touserName;
            this.FromUsername = fromuserName;
        }
    }
}
