using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Collection
    {
        public int CollectionID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public bool Public { get; set; }
        
    }
}
