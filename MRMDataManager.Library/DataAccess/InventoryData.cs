using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;

        public InventoryData(IConfiguration config)
        {
            _config = config;
        }
        public List<InventoryModel> GetInventory()
        {

            SqlDataAccess sqlDataAccess = new SqlDataAccess(_config);

            var output = sqlDataAccess.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "MRMData");

            return output;

        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sqlConn = new SqlDataAccess(_config);

            sqlConn.SaveData("dbo.spInventory_Insert", item, "MRMData");
            
        }
    }
}
