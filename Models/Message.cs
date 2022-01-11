using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloFuture.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public string SenderId { get; set; }
        public bool IsFromSender { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Time { get; set; }
    }
}
