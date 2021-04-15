using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class TagCount
    {
        public string Description { get; set; }
        public int Count { get; set; }
    }
}
