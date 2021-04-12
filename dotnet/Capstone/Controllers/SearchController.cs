﻿using Capstone.Models;
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

        public SearchController(IComicVineService comicVineService, IComicDAO comicDAO)
        {
            this.comicVine = comicVineService;
            this.comicDAO = comicDAO;
        }

        [HttpGet("/issues")]
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

        [HttpGet("local")]
        public ActionResult<List<ComicBook>> LocalComicSearch(string searchTerm = "")
        {
            List<ComicBook> searchResponseList = new List<ComicBook>();
            searchResponseList = comicDAO.LocalComicSearch(searchTerm);
            return Ok(searchResponseList);
        }
    }
}
