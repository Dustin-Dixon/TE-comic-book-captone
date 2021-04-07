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

        private int GetUserIdFromToken()
        {
            string userIdStr = User.FindFirst("sub")?.Value;

            return Convert.ToInt32(userIdStr);
        }

        public UserController(IUserDAO userDAO, ICollectionDAO collectionDAO, IComicDAO comicDAO)
        {
            this.userDAO = userDAO;
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
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
            int userID = GetUserIdFromToken();
            List<ComicBook> comicsInCollection = comicDAO.ComicsInCollection(userID, id);
            return Ok(comicsInCollection);
        }
       [HttpPost("collection/{id}")]
       public ActionResult<ComicBook> AddComicToCollection(int id, ComicBook comicBook)
       {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    comicDAO.AddComicToCollection(id, comicBook);
                    
                    transaction.Complete();
                }
                return Created($"/user/collection/{id}", comicBook);
            }
            catch(Exception)
            {
                return BadRequest(new { message = "Could not add comic to collection" });
            }

       }
    }
   
}
