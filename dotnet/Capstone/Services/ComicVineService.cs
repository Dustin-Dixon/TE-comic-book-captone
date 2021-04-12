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

        public async Task<ComicVineIssueResponse> GetIssues(ComicVineFilters filter)
        {
            IRestRequest request = new RestRequest("/issues");
            if (filter.HasFilters())
            {
                request.AddQueryParameter("filter", filter.GetFiltersString());
            }
            IRestResponse<ComicVineIssueResponse> response = await client.ExecuteGetAsync<ComicVineIssueResponse>(request);
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
    }
}
