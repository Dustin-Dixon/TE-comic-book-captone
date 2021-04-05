using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICollectionDAO
    {
        Collection CreateCollection(int userId, string name);
        Collection ReturnAddedCollection(int id);
        List<Collection> GetAllUserCollections(int userId);
    }
}
