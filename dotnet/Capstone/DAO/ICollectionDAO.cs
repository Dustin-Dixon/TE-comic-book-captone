using Capstone.Models;

namespace Capstone.DAO
{
    public interface ICollectionDAO
    {
        Collection CreateCollection(int userId, string name);
        Collection GetCollections(int id);
    }
}
