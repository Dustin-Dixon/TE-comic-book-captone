using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAO
{
    public interface ICreatorDAO
    {
        public bool AddCreatorCreditToTable(Creator creator);
        public bool LinkCreatorToComic(int creatorId, int comicId);
        public List<Creator> GetComicCreators(int comicId);
        public void CheckDatabaseForCreators(List<Creator> creators);
    }
}
