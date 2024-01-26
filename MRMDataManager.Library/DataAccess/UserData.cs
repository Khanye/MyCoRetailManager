using Microsoft.Extensions.Configuration;
using MRMDataManager.Library.Internal.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;


namespace MRMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sql;

        public UserData(ISqlDataAccess sql)
        {
            _sql = sql;
        }
        public List<UserModel> GetUserById(string Id)
        {
            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserTableLookup", new {Id}, "MRMData");
            return output;
        }

        public void CreateUser(UserModel user)
        {
            _sql.SaveData("dbo.spUser_Insert", new { user.Id, user.FirstName, user.LastName, user.EmailAddress }, "MRMData");
        }
    }
}
