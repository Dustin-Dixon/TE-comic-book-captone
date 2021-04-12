using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Models;
using Capstone.Security;
using System.Collections.Generic;
using System;
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

        public UserController(IUserDAO userDAO, ICollectionDAO collectionDAO, IComicDAO comicDAO)
        {
            this.userDAO = userDAO;
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
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
            if(userRole == "premium")
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
                return Unauthorized(new {message = "Not owner of collection"});
            }
        }

       [HttpPost("collection/{id}")]
       public ActionResult<ComicBook> AddComicToCollection(int id, ComicBook comicBook)
       {
            
            int userId = GetUserIdFromToken();
            if (VerifyActiveUserOwnsCollection(id))
            {
                if (CheckUserRole(userId) || collectionDAO.UserTotalComicCount(userId) < 100  )
                {


                    try
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            bool isSuccessful = comicDAO.AddComicToCollection(id, comicBook);
                            if (!isSuccessful)
                            {
                                return BadRequest(new { message = "Adding a comic was unsuccessful" });
                            }
                            transaction.Complete();
                        }
                        return Created($"/user/collection/{id}", comicBook);
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
                return Unauthorized(new { message = "Unauthorized- Not user collection" });
            }
        }
       
    }
   
}
