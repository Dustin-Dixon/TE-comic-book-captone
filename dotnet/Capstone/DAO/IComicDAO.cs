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
        public bool DeleteComicFromCollection(int collectionId, ComicBook comicBook);
        public int GetComicQuantityInCollection(int collectionId, int comicId);
        bool AddComic(ComicBook comicBook);
        bool AddImages(ComicBook comicBook);
        List<ComicBook> LocalComicSearch(string searchTerm);
        ComicBook GetById(int comicId);
        public bool UpdateQuantityOfComicInCollection(int collectionId, int comicId, int quantity);
    }
}
