using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class CVVolumeResponse : CVResponse
    {
        public Volume Results { get; set; }
    }
}
