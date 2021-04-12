using Capstone.DAO;
using Capstone.Models;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICollectionDAO collectionDAO;
        private readonly IComicDAO comicDAO;
        private readonly IComicVineService comicVine;

        public UserController(ICollectionDAO collectionDAO, IComicDAO comicDAO, IComicVineService comicVine)
        {
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
            this.comicVine = comicVine;
        }
        private int GetUserIdFromToken()
        {
            string userIdStr = User.FindFirst("sub")?.Value;

            return Convert.ToInt32(userIdStr);
        }

        /// <summary>
        /// Gets a collection matching the parameter, 
        /// then compares the current user's id against the collection id.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns>A boolean representing a match or not.</returns>
        private bool VerifyActiveUserOwnsCollection(int collectionId)
        {
            bool userOwns = false;
            Collection collection = collectionDAO.GetSingleCollection(collectionId);
            int userID = GetUserIdFromToken();
            if (userID == collection.UserID)
            {
                userOwns = true;
            }
            return userOwns;
        }

        [HttpGet("collection")]
        public ActionResult<List<Collection>> ListOfCollection()
        {
            int userID = GetUserIdFromToken();
            List<Collection> collections = collectionDAO.GetAllUserCollections(userID);
            return Ok(collections);
        }

        [HttpPost("collection")]
        public ActionResult<Collection> CreateCollection(Collection collection)
        {
            collection.UserID = GetUserIdFromToken();
            collectionDAO.CreateCollection(collection);
            return Created($"/user/collection/{collection.CollectionID}", collection);
        }

        [HttpGet("collection/{id}")]
        public ActionResult<List<ComicBook>> ComicsInCollection(int id)
        {
            if (VerifyActiveUserOwnsCollection(id))
            {
                List<ComicBook> comicsInCollection = comicDAO.ComicsInCollection(id);
                return Ok(comicsInCollection);
            }
            else
            {
                return Unauthorized(new { message = "Not owner of collection" });
            }
        }

        [HttpPost("collection/{id}")]
        public async Task<ActionResult<ComicBook>> AddComicToCollection(int id, ComicBook comicBook)
        {
            if (VerifyActiveUserOwnsCollection(id))
            {
                try
                {
                    ComicBook existing = comicDAO.GetById(comicBook.Id);

                    // Comic book is not in local database, get from API
                    if (existing == null)
                    {
                        ComicVineFilters filters = new ComicVineFilters();
                        filters.AddFilter("id", comicBook.Id.ToString());
                        ComicVineIssueResponse response = await comicVine.GetIssues(filters);
                        if (response.StatusCode != 1)
                        {
                            throw new ComicVineException($"Failed ComicVine request: {response.Error}");
                        }
                        ComicBook issue = response.Results[0];
                        comicDAO.AddComic(issue);
                        existing = issue;
                    }

                    comicDAO.AddComicToCollection(id, existing);

                    return Created($"/user/collection/{id}", comicBook);
                }
                catch (ComicVineException e)
                {
                    return StatusCode(502, new { message = $"Bad Gateway: 502 - {e.Message}" });
                }
                catch (Exception)
                {
                    return BadRequest(new { message = "Could not add comic to collection" });
                }
            }
            else
            {
                return Unauthorized(new { message = "Not owner of collection" });
            }

        }

        [HttpPut("collection/{id}")]
        public ActionResult<Collection> UpdateCollectionPrivacy(int id, Collection collection)
        {
            Collection compareCollection = collectionDAO.GetSingleCollection(id);
            collection.UserID = compareCollection.UserID;
            int userID = GetUserIdFromToken();
            if (userID == collection.UserID)
            {
                int privacyChange = 0;
                if (collection.Public)
                {
                    privacyChange = 1;
                }
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        bool isSuccessful = collectionDAO.UpdateCollectionPrivacy(collection, privacyChange);
                        if (!isSuccessful)
                        {
                            return BadRequest(new { message = "Failed to update collection" });
                        }
                        transaction.Complete();
                    }
                    return Created($"/user/collection/{collection.CollectionID}", collection);
                }
                catch (Exception)
                {
                    return BadRequest(new { message = "Could not update collection privacy" });
                }
            }
            else
            {
                return Unauthorized(new { message = "Unauthorized - Not user collection" });
            }
        }

    }

}
