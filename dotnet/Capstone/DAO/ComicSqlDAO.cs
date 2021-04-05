using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class ComicSqlDAO : IComicDAO
    {
        public ComicBook AddComicToCollection(int collectionId)
        {
            throw new NotImplementedException();
        }

        public List<ComicBook> ComicsInCollection(int userId, int collectionId)
        {
            throw new NotImplementedException();
        }
    }
}
