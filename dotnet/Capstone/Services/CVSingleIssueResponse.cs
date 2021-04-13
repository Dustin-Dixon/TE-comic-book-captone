using Capstone.Models;

namespace Capstone.Services
{
    public class CVSingleIssueResponse
    {
        public string Error { get; set; }
        public int Limit { get; set; }
        public int NumberOfPageResults { get; set; }
        public int NumberOfTotalResults { get; set; }
        public int StatusCode { get; set; }
        public ApiResult Results { get; set; }
    }
}
