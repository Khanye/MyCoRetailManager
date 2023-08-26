using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;


namespace MRMDataManager.Library.DataAccess
{
    public class UserData   
    {
       public List<UserModel> GetUserById(string Id)
        {
            // Direct Dependancy
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };

            var output = sql.LoadData<UserModel,dynamic>("dbo.spUserTableLookup", p, "MRMData");

            return output;
        }
    }
}
