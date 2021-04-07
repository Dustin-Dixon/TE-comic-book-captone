using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IComicDAO
    {

        List<ComicBook> ComicsInCollection(int userId, int collectionId);

        void AddComicToCollection(int collectionId, ComicBook comicBook);

    }
}
