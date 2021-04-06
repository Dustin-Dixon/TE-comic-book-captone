using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IComicDAO
    {

        List<ComicBook> ComicsInCollection(Collection collection);

        void AddComicToCollection(int collectionId, ComicBook comicBook);

    }
}
