using RestSharp;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class ComicVineService : IComicVineService
    {
        private const string URL = "https://comicvine.gamespot.com/api";
        RestClient client;

        public ComicVineService(string apiKey)
        {
            client = new RestClient(URL);
            client.AddDefaultQueryParameter("api_key", apiKey);
            client.AddDefaultQueryParameter("format", "json");
        }

        public async Task<string> GetIssuesInVolume(int volumeId)
        {
            IRestRequest request = new RestRequest("/issues");
            request.AddQueryParameter("filter", $"volume:{volumeId}");
            IRestResponse response = await client.ExecuteGetAsync(request);
            return response.Content;
        }
    }
}
