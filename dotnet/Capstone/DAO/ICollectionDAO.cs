using Capstone.Models;

namespace Capstone.DAO
{
    public interface ICollectionDAO
    {
        Collection CreateCollection(string name);
        Collection GetCollections(int id);
    }
}
