using Capstone.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IComicVineService comicVine;

        public SearchController(IComicVineService comicVineService)
        {
            this.comicVine = comicVineService;
        }

        [HttpGet("issues")]
        public async Task<ActionResult<ComicVineIssueResponse>> SearchIssues(string name, int issueId, int volumeId)
        {
            ComicVineFilters filter = new ComicVineFilters();
            if (name != null)
            {
                filter.AddFilter("name", name);
            }
            if (issueId != 0)
            {
                filter.AddFilter("id", issueId.ToString());
            }
            if (volumeId != 0)
            {
                filter.AddFilter("volume", volumeId.ToString());
            }
            ComicVineIssueResponse response = await comicVine.GetIssues(filter);
            return Ok(response);
        }
    }
}
