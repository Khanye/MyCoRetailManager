using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library.DataAccess
{
    public class ItemData
    {
        public List<ItemModel> GetItems()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ItemModel, dynamic>("dbo.spProductsDisplayAll", new { }, "MRMData");

            return output;

        }
    }
}
