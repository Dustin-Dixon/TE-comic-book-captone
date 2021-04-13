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
        private readonly IUserDAO userDAO;
        private readonly ICollectionDAO collectionDAO;
        private readonly IComicDAO comicDAO;
        private readonly IComicVineService comicVine;

        public UserController(ICollectionDAO collectionDAO, IComicDAO comicDAO, IComicVineService comicVine, IUserDAO userDAO)
        {
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
            this.comicVine = comicVine;
            this.userDAO = userDAO;
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

        private bool CheckUserRole(int userId)
        {
            bool userIsPremium = false;
            User user = userDAO.GetUser(userId);
            string userRole = user.Role;
            if (userRole == "premium")
            {
                userIsPremium = true;
            }
            return userIsPremium;
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
            int userId = GetUserIdFromToken();
            if (VerifyActiveUserOwnsCollection(id))
            {
                if (CheckUserRole(userId) || collectionDAO.UserTotalComicCount(userId) < 100  )
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
                            using(TransactionScope scope = new TransactionScope())
                            {
                                bool addedComic = comicDAO.AddComic(issue);
                                bool addedImages = comicDAO.AddImages(issue);
                                if (addedComic && addedImages)
                                {
                                    scope.Complete();
                                    existing = issue;
                                }
                                else
                                {
                                    throw new Exception("Failed to add new comic from ComicVine API");
                                }
                            }
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
                    return BadRequest(new { message = "Need premium status to add more than 100 comics across all your collections." });
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
                    bool isSuccessful = collectionDAO.UpdateCollectionPrivacy(collection, privacyChange);
                    if (!isSuccessful)
                    {
                        return BadRequest(new { message = "Failed to update collection" });
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
