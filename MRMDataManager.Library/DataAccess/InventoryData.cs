using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library.DataAccess
{
    public  class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {

            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            var output = sqlDataAccess.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "MRMData");

            return output;

        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sqlConn = new SqlDataAccess();

            sqlConn.SaveData("dbo.spInventory_Insert", item, "MRMData");
            
        }
    }
}
