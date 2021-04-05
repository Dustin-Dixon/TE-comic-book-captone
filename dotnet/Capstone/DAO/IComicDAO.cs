using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IComicDAO
    {

        List<ComicBook> ComicsInCollection(int userId, int collectionId);

        ComicBook AddComicToCollection(int collectionId);

    }
}
