using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICollectionDAO
    {
        void CreateCollection(Collection collection);
        List<Collection> GetAllUserCollections(int userId);
        List<Collection> GetPublicCollections();
        bool UpdateCollectionPrivacy(Collection collection, int privacyChange);
        Collection GetSingleCollection(int id);
        int UserTotalComicCount(int userId);
    }
}
