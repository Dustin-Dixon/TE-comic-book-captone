using Microsoft.AspNetCore.Mvc;
using Capstone.DAO;
using Capstone.Models;
using Capstone.Security;
using System.Collections.Generic;
using System;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDAO userDAO;
        private readonly ICollectionDAO collectionDAO;

        private int GetUserIdFromToken()
        {
            string userIdStr = User.FindFirst("sub")?.Value;

            return Convert.ToInt32(userIdStr);
        }

        public UserController(IUserDAO userDAO, ICollectionDAO collectionDAO)
        {
            this.userDAO = userDAO;
            this.collectionDAO = collectionDAO;
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
            return Created($"/user/collection/{collection.CollectionID}",collection);
        }
    }
}
