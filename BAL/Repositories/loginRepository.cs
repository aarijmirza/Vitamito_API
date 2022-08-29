using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BAL.Repositories
{

    public class loginRepository : BaseRepository
    {
        public loginRepository()
            : base()
        {
            DBContext = new db_a74425_premiumposEntities();

        }
        public loginRepository(db_a74425_premiumposEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }
        //public RspBrandList GetBrandInfo()
        //{
        //    var bll = new List<BrandsBLL>();
        //    var lstLoc = new List<LocationsBLL>();
        //    var rsp = new RspBrandList();
        //    try
        //    {
        //        var list = DBContext.sp_getBrands_api().ToList();
        //        var listLocations = DBContext.sp_getLocations_api().ToList();
        //        foreach (var i in list)
        //        {
        //            lstLoc = new List<LocationsBLL>();
        //            foreach (var j in listLocations.Where(x => x.StatusID == 1 && x.BrandID == i.BrandID))
        //            {
        //                lstLoc.Add(new LocationsBLL
        //                {
        //                    ContactNo = j.ContactNo,
        //                    BrandID = j.BrandID,
        //                    Name = j.Name,
        //                    ImageURL = j.ImageURL,
        //                    Email = j.Email,
        //                    Address = j.Address,
        //                    StatusID = j.StatusID,
        //                    DeliveryCharges = j.DeliveryCharges,
        //                    DeliveryTime = j.DeliveryTime,
        //                    MinOrderAmount = j.MinOrderAmount,
        //                    DeliveryServices = j.DeliveryServices,
        //                    LocationID = j.LocationID,
        //                    Latitude = j.Latitude,
        //                    Longitude = j.Longitude
        //                });
        //            }
        //            bll.Add(new BrandsBLL
        //            {

        //                BrandID = i.BrandID,
        //                Username = i.Username,
        //                Name = i.Name,
        //                Image = i.Image == null ? "" : ConfigurationManager.AppSettings["AdminURL"].ToString() + i.Image,
        //                Email = i.Email,
        //                Password = i.Password,
        //                Address = i.Address,
        //                Currency = i.Currency,
        //                BusinessKey = i.BusinessKey,
        //                StatusID = i.StatusID,
        //                Locations = lstLoc,
        //                CompanyURl = i.CompanyURl == null ? "" : ConfigurationManager.AppSettings["AdminURL"].ToString() + i.CompanyURl,
        //                Tax = 15
        //            });
        //        }
        //        rsp.brands = bll;
        //        rsp.status = 1;
        //        rsp.description = "Success";

        //        return rsp;
        //    }
        //    catch (Exception ex)
        //    {
        //        rsp.brands = bll;
        //        rsp.status = 0;
        //        rsp.description = "Failed";
        //        return rsp;
        //    }
        //}
        public RspBanner GetBanners(int UserID,int LocationID)
        {
            var banners = new List<BannerBLL>();
            var rsp = new RspBanner();
            try
            {
                var list = DBContext.Banners.Where(x => x.StatusID == 1  && x.LocationID== LocationID).ToList();

                foreach (var i in list)
                {
                    banners.Add(new BannerBLL
                    {
                        BannerID = i.BannerID,
                        Description = "",
                        Name = i.Title,
                        UserID = UserID,
                        LocationID = i.LocationID,
                        Image = i.Image == null ? "" : ConfigurationManager.AppSettings["AdminURL"].ToString() + i.Image,
                        StatusID = i.StatusID
                    });
                }

                rsp.Banners = banners;
                rsp.status = 1;
                rsp.description = "Success";
                return rsp;
            }
            catch (Exception ex)
            {
                rsp.Banners = banners;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }
        public RspAdminLogin GetLoginAdmin(string passcode)
        {
            var rsp = new RspAdminLogin();
            try
            {
                var data = DBContext.Locations.Where(x => x.Passcode == passcode).FirstOrDefault();
                if (data != null)
                {
                    rsp.LocationID = data.ID;
                    rsp.Longitude = data.Longitude;
                    rsp.Latitude = data.Latitude;
                    rsp.ContactNo = data.ContactNo;
                    rsp.Name = data.Name;
                    rsp.status = 1;
                    rsp.description = "Login successfully.";
                }
                else
                {
                    rsp.status = 0;
                    rsp.description = "User not found.";
                }

                return rsp;
            }
            catch (Exception ex)
            {
                rsp.status = 0;
                rsp.description = "Login failed.";
                return rsp;
            }
        }

        //public Rsp InsertToken(TokenBLL obj)
        //{
        //    Rsp rsp;
        //    try
        //    {
        //        PushToken token = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<PushToken>();
        //        token.StatusID = 1;
        //        var chk = DBContext.PushTokens.Where(x => x.Token == obj.Token).Count();
        //        if (chk==0)
        //        {
        //            PushToken data = DBContext.PushTokens.Add(token);
        //            DBContext.SaveChanges();
        //        }


        //        rsp = new Rsp();
        //        rsp.status = (int)eStatus.Success;
        //        rsp.description = "Token Added";
        //    }
        //    catch (Exception ex)
        //    {
        //        rsp = new Rsp();
        //        rsp.status = (int)eStatus.Exception;
        //        rsp.description = "Failed to add token";
        //    }
        //    return rsp;
        //}
    }
}
