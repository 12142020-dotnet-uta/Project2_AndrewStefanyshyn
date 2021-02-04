using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;

namespace ModelLayer.ViewModels
{
    public class MessagingViewModel
    {
        public string currentUserName { get; set; }
        public int CurrentUserId { get; set; }
        public string friendToMessageUserName { get; set; }
        public int friendToMessageUserId { get; set; }

        public List<Message> messages { get; set; }

    }
}
