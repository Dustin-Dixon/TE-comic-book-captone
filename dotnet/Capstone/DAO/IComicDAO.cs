using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IComicDAO
    {
        List<ComicBook> ComicsInCollection(int collectionId);
        bool AddComicToCollection(int collectionId, ComicBook comicBook);
    }
}
