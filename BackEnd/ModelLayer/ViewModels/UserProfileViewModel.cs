using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;

namespace ModelLayer.ViewModels
{
    public class UserProfileViewModel
    {
        public string userName { get; set; }
        public int userId { get; set; }
        public int numberOfFriends { get; set; }
        public List<Song> top5Songs { get; set; }
        public string FirendStatus { get; set; }

        public UserProfileViewModel() { }
    }
}
