using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class VolumeImages
    {
        public int? VolumeId { get; set; }
        public string IconUrl { get; set; }
        public string SmallUrl { get; set; }
        public string MediumUrl { get; set; }
        public string ThumbUrl { get; set; }
    }
}
