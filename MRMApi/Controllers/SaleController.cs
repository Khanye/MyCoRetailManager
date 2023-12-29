using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRMDataManager.Library.DataAccess;
using MRMDataManager.Library.Models;
using System.Security.Claims;

namespace MRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userid);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSaleReports()
        {
            SaleData data = new SaleData();

            return data.GetSaleReport();

        }
    }
}
