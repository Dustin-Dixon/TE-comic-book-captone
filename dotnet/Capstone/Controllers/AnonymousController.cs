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

        public AnonymousController(ICollectionDAO collectionDAO, IComicDAO comicDAO)
        {
            this.collectionDAO = collectionDAO;
            this.comicDAO = comicDAO;
        }

        [HttpGet("collection")]
        public ActionResult<List<Collection>> GetPublicCollections()
        {
            List<Collection> allPublicCollections = collectionDAO.GetPublicCollections();
            return Ok(allPublicCollections);
        }
    }
}
