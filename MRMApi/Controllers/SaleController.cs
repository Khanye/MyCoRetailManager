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
        private readonly ISaleData _saleData;

        public SaleController(ISaleData saleData )
        {
            _saleData = saleData;
        }
        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _saleData.SaveSale(sale, userid);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        [HttpGet]
        public List<SaleReportModel> GetSaleReports()
        {
            return _saleData.GetSaleReport();
        }

        [AllowAnonymous]
        [Route("GetTaxRate")]
        [HttpGet]
        public decimal GetTaxRate()
        {
            return _saleData.GetTaxRate();
        }
    }
}
