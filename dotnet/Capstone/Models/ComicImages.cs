using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class ComicImages
    {
        public int? ComicId { get; set; }
        public string IconUrl { get; set; }
        public string SmallUrl { get; set; }
        public string MediumUrl { get; set; }
        public string ThumbUrl { get; set; }
    }
}
