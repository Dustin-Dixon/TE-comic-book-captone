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
        private readonly IVolumeDAO volumeDAO;
        private readonly ITagDAO tagDAO;

        public AnonymousController(ICollectionDAO collectionDAO, IComicDAO comicDAO, ICharacterDAO characterDAO, ICreatorDAO creatorDAO, IVolumeDAO volumeDAO, ITagDAO tagDAO)
        {
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
            this.characterDAO = characterDAO;
            this.creatorDAO = creatorDAO;
            this.volumeDAO = volumeDAO;
            this.tagDAO = tagDAO;
        }

        [HttpGet("collection")]
        public ActionResult<List<Collection>> GetPublicCollections()
        {
            List<Collection> allPublicCollections = collectionDAO.GetPublicCollections();
            return Ok(allPublicCollections);
        }

        [HttpGet("stats")]
        public ActionResult<Statistics> GetTotalStatistics()
        {
            return Ok(new Statistics()
            {
                ComicCount = collectionDAO.GetCountOfComicsInAllCollections(),
                Characters = characterDAO.GetTotalCollectionCharacterCount(),
                Creators = creatorDAO.GetTotalCollectionCreatorCount(),
            });
        }

        [HttpGet("collection/{id}/stats")]
        public ActionResult<Statistics> GetPublicCollectionStatistics(int id)
        {
            Collection collection = collectionDAO.GetSingleCollection(id);
            if (collection.Public)
            {
                Statistics stats = new Statistics()
                {
                    ComicCount = collectionDAO.GetCountOfComicsInCollection(id),
                    Characters = characterDAO.GetCollectionCharacterCount(id),
                    Creators = creatorDAO.GetCollectionCreatorCount(id),
                };
                return Ok(stats);
            }
            else
            {
                return Unauthorized(new { message = "The collection is private" });
            }
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
                    comic.Volume = volumeDAO.GetComicVolume(comic.Id);
                    comic.Tags = tagDAO.GetTagListForComicBook(comic.Id);
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
