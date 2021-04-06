using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IComicDAO
    {

        List<ComicBook> ComicsInCollection(Collection collection);

        ComicBook AddComicToCollection(int collectionId);

    }
}
