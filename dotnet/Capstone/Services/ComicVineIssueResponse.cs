using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Services
{
    public class ComicVineIssueResponse
    {
        public string Error { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int NumberOfPageResults { get; set; }
        public int NumberOfTotalResults { get; set; }
        public int StatusCode { get; set; }
        public List<ComicBook> Results { get; set; }
    }
}
