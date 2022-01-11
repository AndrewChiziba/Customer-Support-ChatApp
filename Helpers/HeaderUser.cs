using HelloFuture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloFuture.Helpers
{
    public class HeaderUser
    {
        public HeaderUser()
        {
            Messages = new HashSet<Message>();
        }
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string RecieverId { get; set; }
        public string RecieverUsername { get; set; }
        
        public DateTime Time { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
