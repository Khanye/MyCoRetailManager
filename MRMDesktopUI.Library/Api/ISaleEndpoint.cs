using MRMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace MRMDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}