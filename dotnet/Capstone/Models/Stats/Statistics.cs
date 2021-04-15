using Capstone.Models.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Statistics
    {
        public int ComicCount { get; set; }
        public List<CharacterCount> Characters { get; set; }
    }
}
