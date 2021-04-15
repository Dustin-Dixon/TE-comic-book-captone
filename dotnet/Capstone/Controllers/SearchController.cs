using Capstone.Models;
using Capstone.DAO;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IComicVineService comicVine;
        private readonly IComicDAO comicDAO;
        private readonly ICharacterDAO characterDAO;
        private readonly ICreatorDAO creatorDAO;
        private readonly IVolumeDAO volumeDAO;
        private readonly ITagDAO tagDAO;

        public SearchController(IComicVineService comicVineService, IComicDAO comicDAO, ICharacterDAO characterDAO, ICreatorDAO creatorDAO, IVolumeDAO volumeDAO, ITagDAO tagDAO)
        {
            comicVine = comicVineService;
            this.comicDAO = comicDAO;
            this.characterDAO = characterDAO;
            this.creatorDAO = creatorDAO;
            this.volumeDAO = volumeDAO;
            this.tagDAO = tagDAO;
        }

        [HttpGet("issues")]
        public async Task<ActionResult<CVIssueResponse>> SearchIssues(string name,string description)
        {
            ComicVineFilters filter = new ComicVineFilters();
            if (name != null)
            {
                filter.AddFilter("name", name);
            }
            if (description != null)
            {
                filter.AddFilter("description", description);
            }
            CVIssueResponse response = await comicVine.GetIssues(filter);
            return Ok(response);
        }

        [HttpGet("local")]
        public ActionResult<List<ComicBook>> LocalComicSearch(string searchTerm = "")
        {
            List<ComicBook> searchResponseList = new List<ComicBook>();
            searchResponseList = comicDAO.LocalComicSearch(searchTerm);
            foreach (ComicBook comic in searchResponseList)
            {
                comic.Characters = characterDAO.GetCharacterListForComicBook(comic.Id);
                comic.Creators = creatorDAO.GetComicCreators(comic.Id);
                comic.Volume = volumeDAO.GetComicVolume(comic.Id);
                comic.Tags = tagDAO.GetTagListForComicBook(comic.Id);
            }
            return Ok(searchResponseList);
        }
    }
}
