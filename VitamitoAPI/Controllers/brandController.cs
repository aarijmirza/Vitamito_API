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
    public class brandController : ApiController
    {
        loginRepository loginRepo;
        /// <summary>
        /// 
        /// </summary>
        public brandController()
        {
            loginRepo = new loginRepository(new db_a74425_premiumposEntities());

        }


#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Get list of Brands and Locations are inherited in each brand model
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("brand/all")]
        //public RspBrandList GetBrands()
        //{
        //    return loginRepo.GetBrandInfo();
        //}


        /// <summary>
        /// Get list for dashboard w.r.t brands selected
        /// </summary>
        /// <returns></returns>
        [HttpGet]
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
        [Route("banners/all/{UserID}")]
        public Rsp GetBanners(int UserID)
        {
            return loginRepo.GetBanners(UserID,0);
        }

        [HttpGet]
        [Route("banners/all/{UserID}/{LocationID}")]
        public Rsp GetBannersv2(int UserID, int LocationID)
        {
            return loginRepo.GetBanners(UserID, LocationID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("settings/user/{UserID}")]
        public AppSettings GetSettings(int UserID)
        {
            return new AppSettings{
            Currency="Rs",
            DeliveryCharges="0",
            LocationID= 2182,
            Status="1",
            TaxPercent="0",
            UserID= 2313
            };
        }
    }
}
