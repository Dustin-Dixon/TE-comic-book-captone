using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.Stats
{
    public class CharacterCount
    {
        public Character Character { get; set; }
        public int Count { get; set; } = 0;
    }
}
