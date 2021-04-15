using System.Threading.Tasks;

namespace Capstone.Services
{
    public interface IComicVineService
    {
        public Task<CVIssueResponse> GetIssues(ComicVineFilters filter);
        public Task<CVSingleIssueResponse> GetIssueDetails(string comic_api_url);
        public Task<CVVolumeResponse> GetVolumeDetails(string volume_api_url);
    }
}
