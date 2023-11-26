using Microsoft.AspNet.Identity;
using MRMDataManager.Library.DataAccess;
using MRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MRMDataManager.Controllers
{
    public class UserController : ApiController
    {
        [Authorize]       
        // GET: api/User/5
        public UserModel GetById()
        {
            string userid = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById (userid).First();           
        }

    }
}
