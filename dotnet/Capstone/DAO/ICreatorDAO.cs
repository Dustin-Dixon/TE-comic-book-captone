using Capstone.Models;
using Capstone.Models.Stats;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICreatorDAO
    {
        public bool AddCreatorCreditToTable(Creator creator);
        public bool LinkCreatorToComic(int creatorId, int comicId);
        public List<Creator> GetComicCreators(int comicId);
        public void CheckDatabaseForCreators(List<Creator> creators);
        public List<CreatorCount> GetCollectionCreatorCount(int collectionId);
        public List<CreatorCount> GetTotalCollectionCreatorCount();
    }
}
