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
    //[Authorize]
    public class ItemController : ApiController
    {
        public List<ItemModel> Get()
        {
            ItemData data = new ItemData();

            return data.GetItems();

        }

    }
}
