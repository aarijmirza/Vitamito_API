using BAL.Repositories;
using DAL.DBEntities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VitamitoAPI.Controllers
{
    [RoutePrefix("api")]
    public class menuController : ApiController
    {

        menuRepository repo;
        public menuController()
        {
            repo = new menuRepository(new db_a74425_premiumposEntities());
        }
        [Route("menu/{locationID}/{UserID}")]
        public RspMenu GetMenu(string locationID, int UserID)
        {
            return repo.GetMenu(int.Parse(locationID), UserID);
        }
    }
}
