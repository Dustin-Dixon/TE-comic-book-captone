﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        internal bool InDatabase { get; set; } = false;
    }
}
