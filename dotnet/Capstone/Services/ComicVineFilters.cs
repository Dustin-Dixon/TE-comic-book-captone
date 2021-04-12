using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class ComicVineFilters
    {
        private List<string> filters = new List<string>();

        public void AddFilter(string name, string value)
        {
            filters.Add($"{name}:{value}");
        }
        public bool HasFilters()
        {
            return (filters.Count != 0);
        }
        public string GetFiltersString()
        {
            return string.Join(",", filters);
        }
    }
}
