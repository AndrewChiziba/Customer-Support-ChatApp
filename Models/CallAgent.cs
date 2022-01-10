using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloFuture.Models
{
    public class CallAgent
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool available { get; set; }
    }
}
