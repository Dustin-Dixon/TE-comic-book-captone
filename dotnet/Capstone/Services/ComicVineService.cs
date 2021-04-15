using RestSharp;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class ComicVineService : IComicVineService
    {
        private const string URL = "https://comicvine.gamespot.com/api";
        readonly RestClient client;

        public ComicVineService(string apiKey)
        {
            client = new RestClient(URL);
            client.AddDefaultQueryParameter("api_key", apiKey);
            client.AddDefaultQueryParameter("format", "json");
        }

        public async Task<CVIssueResponse> GetIssues(ComicVineFilters filter)
        {
            IRestRequest request = new RestRequest("/issues");
            if (filter.HasFilters())
            {
                request.AddQueryParameter("filter", filter.GetFiltersString());
            }
            IRestResponse<CVIssueResponse> response = await client.ExecuteGetAsync<CVIssueResponse>(request);
            HandleError(response);
            return response.Data;
        }

        private void HandleError(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new ComicVineException($"Failed to connect to ComicVine API: {response.ErrorMessage}");
            }
            else if (!response.IsSuccessful)
            {
                throw new ComicVineException($"Unsuccessful request to ComicVineAPI: Status {response.StatusCode} - {response.StatusDescription}");
            }
        }

        public async Task<CVSingleIssueResponse> GetIssueDetails(string comic_api_url)
        {
            IRestRequest request = new RestRequest(SliceComicApiUrl(comic_api_url));
            IRestResponse<CVSingleIssueResponse> response = await client.ExecuteGetAsync<CVSingleIssueResponse>(request);
            HandleError(response);
            return response.Data;
        }

        public async Task<CVVolumeResponse> GetVolumeDetails(string volume_api_url)
        {
            IRestRequest request = new RestRequest(SliceComicApiUrl(volume_api_url));
            IRestResponse<CVVolumeResponse> response = await client.ExecuteGetAsync<CVVolumeResponse>(request);
            HandleError(response);
            return response.Data;
        }

        private string SliceComicApiUrl(string comic_api_url)
        {
            string slice = comic_api_url.Substring(35);
            return slice;
        }
    }
}
