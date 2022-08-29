using BAL.Repositories;
using DAL.DBEntities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace VitamitoAPI.Controllers
{
    [RoutePrefix("api")]
    public class customerController : ApiController
    {
        customerRepository repo;
        /// <summary>
        /// 
        /// </summary>
        public customerController()
        {
            repo = new customerRepository(new db_a74425_premiumposEntities());
        }

        /// <summary>
        /// List of categories and for each category , item list is inherited
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("customer/login/{username}/{password}/{type}")]
        public RspCustomerLogin loginCustomer(string username, string password,string type)
        {
            return repo.GetCustomerInfo(username, password, type);

        }

        //===============================================================
        /// <summary>
        /// List of categories and for each category , item list is inherited
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("customer/login/{CustomerID}")]
        public RspCustomerLogin loginCustomer(int CustomerID)
        {
            return repo.GetCustomerInfo(CustomerID);

        }


        //===============================================================

        /// <summary>
        /// Reset customer password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customer/forgetpassword/{email}")]
        public Rsp loginCustomer(string email)
        {
            return repo.ForgetPassword(email);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customer/signup")]
        public HttpResponseMessage PostSignUp(CustomerBLL obj)
        {
            RspCustomerSignup rsp = new RspCustomerSignup();
            try
            {
                rsp = repo.Signup(obj);
            }
            catch (Exception ex)
            {
                rsp = new RspCustomerSignup();
                rsp.status = (int)eStatus.Exception;
                rsp.description = ex.Message;
                rsp.CustomerID = 0;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(rsp);
            json = Newtonsoft.Json.Linq.JObject.Parse(json).ToString();
            return new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "text/json")    //  RETURNING json
            };

        }


        /// <summary>
        /// Customer Address Insert
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customer/address/addorupdate")]
        public HttpResponseMessage PostCustomerAddress(CustomerAddressBLL obj)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(repo.AddOrUpdateAddress(obj));
            json = Newtonsoft.Json.Linq.JObject.Parse(json).ToString();
            return new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "text/json")    //  RETURNING json
            };

        }


        /// <summary>
        /// Customer Payment Add
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("customer/payment/add")]
        //public HttpResponseMessage PostCustomerPayment(CustomerPaymentBLL obj)
        //{
        //    RspCustomerPayment rsp = new RspCustomerPayment();
        //    try
        //    {
        //        rsp = repo.Insert(obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        rsp = new RspCustomerPayment();
        //        rsp.status = (int)eStatus.Exception;
        //        rsp.description = ex.Message;
        //        rsp.PaymentID = 0;
        //    }
        //    string json = Newtonsoft.Json.JsonConvert.SerializeObject(rsp);
        //    json = Newtonsoft.Json.Linq.JObject.Parse(json).ToString();
        //    return new HttpResponseMessage
        //    {
        //        Content = new StringContent(json, Encoding.UTF8, "text/json")    //  RETURNING json
        //    };

        //}
    }
}
    