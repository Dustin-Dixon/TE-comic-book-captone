﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Creator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        internal bool InDatabase { get; set; } = false;
    }
}
