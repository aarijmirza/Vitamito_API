using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BAL.Repositories
{

    public class customerRepository : BaseRepository
    {

        public customerRepository()
            : base()
        {
            DBContext = new db_a74425_premiumposEntities();

        }

        public customerRepository(db_a74425_premiumposEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }

        public RspCustomerLogin GetCustomerInfo(int CustomerID)
        {
            var bll = new CustomerBLL();
            var objCustomerDetail = new CustomerDetailBLL();
            var lstAddress = new List<CustomerAddressBLL>();
            var lstPayment = new List<CustomerPaymentBLL>();
            var rsp = new RspCustomerLogin();
            try
            {

                var customerInfo = new Customer();
                customerInfo = DBContext.Customers.Where(x => x.StatusID == 1 && x.ID == CustomerID).FirstOrDefault();
                if (customerInfo == null)
                {
                    rsp.customer = null;
                    rsp.status = 0;
                    rsp.description = "username or password is not correct";
                    return rsp;
                }
                
                var addresses = customerInfo.CustomerAddresses.Where(x => x.StatusID == 1).OrderByDescending(x => x.CustomerAddressID).ToList();

                foreach (var k in addresses)
                {
                    lstAddress.Add(new CustomerAddressBLL
                    {
                        StatusID = k.StatusID,
                        CustomerAddressID = k.CustomerAddressID,
                        Address = k.Address,
                        NickName = k.NickName,
                        Latitude = k.Latitude,
                        Longitude = k.Longitude,
                        ContactNo = k.ContactNo,
                        CustomerID = k.CustomerID,
                        Country = k.Country,
                        StreetName = k.StreetName
                    });
                }
                lstPayment = new List<CustomerPaymentBLL>();
                //temp
                //foreach (var k in result.CustomerPayments.Where(x => x.StatusID == 1))
                //{
                //    lstPayment.Add(new CustomerPaymentBLL
                //    {
                //        StatusID = k.StatusID,
                //        BrandID = k.BrandID,
                //        CustomerID = k.CustomerID,
                //        CardExpire = k.CardExpire,
                //        CardTitle = k.CardTitle,
                //        CVV = k.CVV,
                //        Description = k.Description,
                //        Name = k.Name,
                //        PaymentID = k.PaymentID,
                //    });
                //}
                objCustomerDetail = null;

                rsp.customer = new CustomerBLL
                {
                    Addresses = lstAddress,
                    Cards = lstPayment,
                    UserID = customerInfo.UserID,
                    CustomerID = customerInfo.ID,
                    Password = customerInfo.Password,
                    Email = customerInfo.Email,
                    FullName = customerInfo.FullName,
                    Image = customerInfo.Image,
                    LastUpdatedBy = customerInfo.LastUpdatedBy,
                    LastUpdatedDate = customerInfo.LastUpdatedDate,
                    Mobile = customerInfo.Mobile,
                    StatusID = customerInfo.StatusID
                };

                rsp.status = 1;
                rsp.description = "Success";
                return rsp;
            }
            catch (Exception ex)
            {
                rsp.customer = null;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }

        public RspCustomerLogin GetCustomerInfo(string username, string password, string type)
        {
            var bll = new CustomerBLL();
            var objCustomerDetail = new CustomerDetailBLL();
            var lstAddress = new List<CustomerAddressBLL>();
            var lstPayment = new List<CustomerPaymentBLL>();
            var rsp = new RspCustomerLogin();
            try
            {
                var customerInfo = new Customer();
                if (type == "sm")
                {
                    customerInfo = DBContext.Customers.Where(x => x.Email == username).FirstOrDefault();
                    if (customerInfo == null)
                    {
                        customerInfo = new Customer();
                        var currDate = DateTime.UtcNow.AddMinutes(180);
                        customerInfo.Email = username;
                        customerInfo.LastUpdatedDate = DateTime.UtcNow.AddMinutes(180);
                        customerInfo.StatusID = 1;
                        customerInfo = DBContext.Customers.Add(customerInfo);
                        DBContext.SaveChanges();
                    }
                }
                else
                {
                    customerInfo = DBContext.Customers.Where(x => x.StatusID == 1 && (x.Email == username || x.Mobile == username)
                   && x.Password == password
                   ).FirstOrDefault();
                    if (customerInfo == null)
                    {
                        rsp.customer = null;
                        rsp.status = 0;
                        rsp.description = "username or password is not correct";
                        return rsp;
                    }
                }
                var addresses = customerInfo.CustomerAddresses.Where(x => x.StatusID == 1).OrderByDescending(x => x.CustomerAddressID).ToList();

                foreach (var k in addresses)
                {
                    lstAddress.Add(new CustomerAddressBLL
                    {
                        StatusID = k.StatusID,
                        CustomerAddressID = k.CustomerAddressID,
                        Address = k.Address,
                        NickName = k.NickName,
                        Latitude = k.Latitude,
                        Longitude = k.Longitude,
                        ContactNo = k.ContactNo,
                        CustomerID = k.CustomerID,
                        Country = k.Country,
                        StreetName = k.StreetName
                    });
                }
                lstPayment = new List<CustomerPaymentBLL>();
                //temp
                //foreach (var k in result.CustomerPayments.Where(x => x.StatusID == 1))
                //{
                //    lstPayment.Add(new CustomerPaymentBLL
                //    {
                //        StatusID = k.StatusID,
                //        BrandID = k.BrandID,
                //        CustomerID = k.CustomerID,
                //        CardExpire = k.CardExpire,
                //        CardTitle = k.CardTitle,
                //        CVV = k.CVV,
                //        Description = k.Description,
                //        Name = k.Name,
                //        PaymentID = k.PaymentID,
                //    });
                //}


                objCustomerDetail = null;

                rsp.customer = new CustomerBLL
                {
                    Addresses = lstAddress,
                    Cards = lstPayment,
                    UserID = customerInfo.UserID,
                    CustomerID = customerInfo.ID,
                    Password = customerInfo.Password,
                    Email = customerInfo.Email,
                    FullName = customerInfo.FullName,
                    Image = customerInfo.Image,
                    LastUpdatedBy = customerInfo.LastUpdatedBy,
                    LastUpdatedDate = customerInfo.LastUpdatedDate,
                    Mobile = customerInfo.Mobile,
                    StatusID = customerInfo.StatusID
                };

                rsp.status = 1;
                rsp.description = "Success";
                return rsp;
            }
            catch (Exception ex)
            {
                rsp.customer = null;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }

        public Rsp ForgetPassword(string email)
        {
            var bll = new CustomerBLL();
            var rsp = new Rsp();
            try
            {

                var result = DBContext.Customers.Where(x => x.StatusID == 1 && x.Email == email).FirstOrDefault();

                result.Password = RandomString(10, false);
                DBContext.Customers.Attach(result);
                DBContext.UpdateOnly<SubUser>(
                    result, x => x.Password);

                DBContext.SaveChanges();

                try
                {
                    Email("Car Express",
                        "Your password has been reset successfully./n your new password is " + result.Password + ".",
                        email,
                        "",
                        "",
                        "",
                        0
                        );
                }
                catch
                {
                }

                rsp.status = 1;
                rsp.description = "Password reset successfully";


                return rsp;
            }
            catch (Exception ex)
            {
                rsp.status = 0;
                rsp.description = "Failed to ";
                return rsp;
            }
        }

        public RspCustomerSignup Signup(CustomerBLL obj)
        {
            RspCustomerSignup rsp;

            try
            {
                using (var dbContextTransaction = DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.CustomerID != 0)
                        {
                            var customer = DBContext.Customers.Where(x => x.ID == obj.CustomerID).FirstOrDefault();

                            customer.Email = obj.Email;
                            customer.Password = obj.Password;
                            customer.Mobile = obj.Mobile;
                            customer.FullName = obj.FullName;

                            DBContext.Customers.Attach(customer);
                            DBContext.UpdateExcept<Customer>(
                                customer, x => x.ID);
                            DBContext.SaveChanges();
                            dbContextTransaction.Commit();

                            rsp = new RspCustomerSignup();
                            rsp.status = (int)eStatus.Success;
                            rsp.description = "Your profile has been updated successfully.";
                            rsp.CustomerID = obj.CustomerID;
                        }
                        else
                        {

                            var chkCustomer = DBContext.Customers.Where(x => (x.Mobile == obj.Mobile || x.Email == obj.Email) && x.Password == obj.Password).Count();

                            if (chkCustomer == 0)
                            {
                                var currDate = DateTime.UtcNow.AddMinutes(300);

                                Customer customer = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<Customer>();
                                customer.LastUpdatedDate = DateTime.UtcNow.AddMinutes(300);
                                customer.StatusID = 1;
                                Customer data = DBContext.Customers.Add(customer);
                                DBContext.SaveChanges();
                                dbContextTransaction.Commit();

                                rsp = new RspCustomerSignup();
                                rsp.status = (int)eStatus.Success;
                                rsp.description = "Your account has been registered successfully.";
                                rsp.CustomerID = data.ID;
                            }
                            else
                            {
                                rsp = new RspCustomerSignup();
                                rsp.status = (int)eStatus.Failed;
                                rsp.description = "Username or password is already registered";
                                rsp.CustomerID = 0;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        rsp = new RspCustomerSignup();
                        rsp.status = (int)eStatus.Exception;
                        rsp.description = "Sorry, You cannot be able to signup now.";
                        rsp.CustomerID = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                rsp = new RspCustomerSignup();
                rsp.status = (int)eStatus.Exception;
                rsp.description = "Sorry, You cannot be able to signup now.";
                rsp.CustomerID = 0;
            }
            return rsp;
        }
        public RspCustomerAddress AddOrUpdateAddress(CustomerAddressBLL obj)
        {
            RspCustomerAddress rsp;
            using (var dbContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    obj.CustomerAddressID = obj.CustomerAddressID == null ? 0 : obj.CustomerAddressID;
                    CustomerAddress address = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<CustomerAddress>();
                    address.StatusID = address.StatusID == null ? 1 : address.StatusID;
                    DBContext.CustomerAddresses.AddOrUpdate(address);
                    DBContext.SaveChanges();
                    dbContextTransaction.Commit();
                    rsp = new RspCustomerAddress();
                    rsp.status = (int)eStatus.Success;
                    rsp.description = "Your addresses updated successfully.";
                    rsp.AddressID = 0;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    rsp = new RspCustomerAddress();
                    rsp.status = (int)eStatus.Exception;
                    rsp.description = "Something went wrong.";
                    rsp.AddressID = 0;
                }
            }

            return rsp;
        }
        //public RspCustomerPayment Insert(CustomerPaymentBLL obj)
        //{
        //    RspCustomerPayment rsp;

        //    using (var dbContextTransaction = DBContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (obj.PaymentID == 0)
        //            {
        //                var currDate = DateTime.UtcNow.AddMinutes(180);

        //                CustomerPayment payment = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<CustomerPayment>();
        //                payment.StatusID = 1;
        //                CustomerPayment data = DBContext.CustomerPayments.Add(payment);
        //                DBContext.SaveChanges();
        //                dbContextTransaction.Commit();

        //                rsp = new RspCustomerPayment();
        //                rsp.status = (int)eStatus.Success;
        //                rsp.description = "Your payment added successfully.";
        //                rsp.PaymentID = data.PaymentID;
        //            }
        //            else
        //            {
        //                var currDate = DateTime.UtcNow.AddMinutes(180);

        //                CustomerPayment payment = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<CustomerPayment>();
        //                payment.StatusID = 1;
        //                DBContext.CustomerPayments.AddOrUpdate(payment);
        //                DBContext.SaveChanges();
        //                dbContextTransaction.Commit();

        //                rsp = new RspCustomerPayment();
        //                rsp.status = (int)eStatus.Success;
        //                rsp.description = "Your payment updated successfully.";
        //                rsp.PaymentID = obj.PaymentID;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            dbContextTransaction.Rollback();
        //            rsp = new RspCustomerPayment();
        //            rsp.status = (int)eStatus.Exception;
        //            rsp.description = "Your address doesnot added successfully.";
        //            rsp.PaymentID = 0;
        //        }
        //    }

        //    return rsp;
        //}
    }
}
