using System.Threading.Tasks;

namespace Capstone.Services
{
    public interface IComicVineService
    {
        public Task<ComicVineIssueResponse> GetIssues(ComicVineFilters filter);
    }
}
