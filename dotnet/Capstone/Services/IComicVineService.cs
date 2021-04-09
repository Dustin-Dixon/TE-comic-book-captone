using System.Threading.Tasks;

namespace Capstone.Services
{
    public interface IComicVineService
    {
        public Task<string> GetIssuesInVolume(int volumeId);
    }
}
