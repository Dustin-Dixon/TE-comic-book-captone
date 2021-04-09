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

        [HttpGet("")]
        public async Task<ActionResult> TestSearch()
        {
            string response = await comicVine.GetIssuesInVolume(796);
            return Ok(response);
        }
    }
}
