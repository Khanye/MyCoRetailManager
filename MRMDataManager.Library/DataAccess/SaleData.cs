using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //Make this SOLID/Dry/Better
            // Start filling in the sale detail models we will save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData product = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail  = new SaleDetailDBModel
                {
                    ProductId = item.ProductID,
                    Quantity = item.Quantity,
                };

                //Get more information about this product from the database
                // ProductData product = new ProductData();

                 var productinfo = product.GetProductById(item.ProductID);

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
            // Save the SaleModel
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData<SaleDBModel>("dbo.spSale_Insert", sale,"MRMData");
            // Get the ID from the SaleModel

            sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new { sale.CashierId,sale.SaleDate }, "MRMData").FirstOrDefault();
            // Finish filling in the SaleDetail Model

            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                // Save the sale detail models
                sql.SaveData("dbo.spSaleDetail_Insert", item, "MRMData");
            }
        }
    }
}
