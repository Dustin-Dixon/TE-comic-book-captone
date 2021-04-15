using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Services
{
    public class CVIssueResponse : CVResponse
    {
        public List<ComicBook> Results { get; set; }
    }
}
