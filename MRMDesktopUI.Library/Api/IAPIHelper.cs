using MRMDesktopUI.Library.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MRMDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        void LogOffUser();
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task  GetLoggedInUserInfo(string token);
    }
}