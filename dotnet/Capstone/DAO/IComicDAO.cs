using Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public interface IComicDAO
    {
        List<ComicBook> ComicsInCollection(int collectionId);
        bool AddComicToCollection(int collectionId, ComicBook comicBook);
        bool AddComic(ComicBook comicBook);
        List<ComicBook> LocalComicSearch(string searchTerm);
        ComicBook GetById(int comicId);
    }
}
