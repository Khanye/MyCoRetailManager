using Microsoft.Extensions.Configuration;
using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly IProductData _productData;
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _config;

        public SaleData(IProductData productData, ISqlDataAccess sql,IConfiguration config)
        {
            _productData = productData;
            _sql = sql;
            _config = config;
        }

        public decimal GetTaxRate()
        {
            string taxRateText = _config.GetValue<string>("TaxRate"); 
            bool IsValidTaxRate = Decimal.TryParse(taxRateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            output = output / 100;
            return output;
        }
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //Make this SOLID/Dry/Better
            // Start filling in the sale detail models we will save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            var taxRate = GetTaxRate();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductID,
                    Quantity = item.Quantity,
                };

                //Get more information about this product from the database
                var productinfo = _productData.GetProductById(item.ProductID);

                if (productinfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductID} could not be found in the database.");
                }
                else
                {
                    detail.PurchasePrice = (productinfo.RetailPrice * detail.Quantity);
                    if (productinfo.IsTaxable)
                    {
                        detail.Tax = (detail.PurchasePrice * taxRate);
                    }

                }

                details.Add(detail);
            }
            // Create the SaleModel
            SaleDBModel sale = new SaleDBModel
            {
                Subtotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.Subtotal + sale.Tax;

            try
            {
                _sql.StartTransaction("MRMData");
                // Save the SaleModel
                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);
                // Get the ID from the SaleModel 
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
                // Finish filling in the SaleDetail Model
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    // Save the sale detail models
                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }

                _sql.CommitTransaction();
            }
            catch
            {
                _sql.RollbackTransaction();
                throw;
            }

        }

        public List<SaleReportModel> GetSaleReport()
        {
            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "MRMData");
            return output;
        }
    }
}
