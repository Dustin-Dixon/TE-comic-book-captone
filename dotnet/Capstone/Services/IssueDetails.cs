﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Services
{
    public class IssueDetails
    {
        public List<Character> CharacterCredits { get; set; }
        public List<Creator> PersonCredits { get; set; }
        public Volume Volume { get; set; }
    }
}
