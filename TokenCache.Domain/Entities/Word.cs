using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Domain.Entities
{
    public class Word
    {
        public int Id { get; set; }
        public  int UserId { get; set; }
        public string WordText { get; set; }     
        public User User { get; set; }
    }
}
