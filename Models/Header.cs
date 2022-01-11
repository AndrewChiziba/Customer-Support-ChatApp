using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloFuture.Models
{
    public class Header
    {
        public Header()
        {
            Messages = new HashSet<Message>();
        }
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        //public Product Subject { get; set; }
        public DateTime Time { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
