using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.DAO;
using Capstone.Models;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnonymousController : ControllerBase
    {
        private readonly ICollectionDAO collectionDAO;
        private readonly IComicDAO comicDAO;
        private readonly ICharacterDAO characterDAO;
        private readonly ICreatorDAO creatorDAO;

        public AnonymousController(ICollectionDAO collectionDAO, IComicDAO comicDAO, ICharacterDAO characterDAO, ICreatorDAO creatorDAO)
        {
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
            this.characterDAO = characterDAO;
            this.creatorDAO = creatorDAO;
        }

        [HttpGet("collection")]
        public ActionResult<List<Collection>> GetPublicCollections()
        {
            List<Collection> allPublicCollections = collectionDAO.GetPublicCollections();
            return Ok(allPublicCollections);
        }

        [HttpGet("collection/statistics")]
        public ActionResult<int> GetNumberOfComicsInCollections()
        {
            int overallNumber = collectionDAO.GetCountOfComicsInAllCollections();
            return Ok(overallNumber);
        } 

        [HttpGet("collection/{id}/comic")]
        public ActionResult<List<ComicBook>> ComicsInPublicCollection(int id)
        {
            Collection collection = collectionDAO.GetSingleCollection(id);
            if (collection.Public)
            {
                List<ComicBook> publicViewComics = comicDAO.ComicsInCollection(id);
                foreach (ComicBook comic in publicViewComics)
                {
                    comic.Characters = characterDAO.GetCharacterListForComicBook(comic.Id);
                    comic.Creators = creatorDAO.GetComicCreators(comic.Id);
                }
                return Ok(publicViewComics);
            }
            else
            {
                return Unauthorized(new {message = "This collection is private"});
            }
        }

        [HttpGet("collection/{id}")]
        public ActionResult<Collection> GetSpecificCollection (int id)
        {
            Collection collection = collectionDAO.GetSingleCollection(id);
            if (collection.Public)
            {
                return Ok(collection);
            }
            else
            {
                return Unauthorized(new { message = "This collection is private" });
            }
        }
    }
}
