using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Domain.Entities
{
    public class RedisCacheRequestModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
