using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using WebAPICode.Helpers;

namespace BAL.Repositories
{
    public class orderRepository : BaseRepository
    {

        public orderRepository()
            : base()
        {
            DBContext = new db_a74425_premiumposEntities();
        }

        public orderRepository(db_a74425_premiumposEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }

        public RspOrdersCustomer GetOrderCustomersV2(int orderID, int customerID)
        {
            var bll = new List<OrdersBLL>();
            var lstOD = new List<OrderDetailBLL>();
            var lstODM = new List<OrderModifierDetailBLL>();
            var oc = new OrderCheckoutBLL();
            var ocustomer = new OrderCustomerBLL();
            var rsp = new RspOrdersCustomer();
            var Modifier = new ModifierBLL();
            var Variant = new VariantsBLL();
            try
            {
                var ds = GetCustomerOrder_ADO(customerID);
                var _dsOrders = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<OrdersBLL>>();
                var _dsorderdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<OrderDetailBLL>>();
                var _dsOrdercheckout = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OrderCheckoutBLL>>();
                var _dsOrderCustomerData = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OrderCustomerBLL>>();
                var _dsorderdetailmodifier = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<OrderModifierDetailBLL>>();
                

                foreach (var i in _dsOrders)
                {
                    lstOD = new List<OrderDetailBLL>();
                    foreach (var j in _dsorderdetail.Where(x => x.StatusID == 1 && x.OrderID == i.ID))
                    {
                        lstODM = new List<OrderModifierDetailBLL>();

                        var ODM_modifier = _dsorderdetailmodifier.Where(x => x.StatusID == 1 && x.OrderDetailID == j.ID && x.Type == "Modifier").FirstOrDefault();
                        var ODM_Variant = _dsorderdetailmodifier.Where(x => x.StatusID == 1 && x.OrderDetailID == j.ID && x.Type == "Variant").FirstOrDefault();

                        if (ODM_modifier!=null)
                        {
                            var k = ODM_modifier;
                            lstODM.Add(new OrderModifierDetailBLL
                            {
                                StatusID = k.StatusID,
                                Price = k.Price,
                                ModifierID = k.ModifierID,
                                VariantID = k.VariantID,
                                Type = k.Type,
                                Cost = k.Cost,
                                OrderDetailID = k.OrderDetailID,
                                OrderModifierDetailID = k.OrderModifierDetailID,
                                ModifierName = k.ModifierName,

                            });

                        }

                        if (ODM_Variant != null)
                        {
                            var k = ODM_Variant;
                            lstODM.Add(new OrderModifierDetailBLL
                            {
                                StatusID = k.StatusID,
                                Price = k.Price,
                                ModifierID = k.ModifierID,
                                VariantID = k.VariantID,
                                Type = k.Type,
                                Cost = k.Cost,
                                OrderDetailID = k.OrderDetailID,
                                OrderModifierDetailID = k.OrderModifierDetailID,
                                VariantName = k.VariantName,

                            });

                        }
                       

                        lstOD.Add(new OrderDetailBLL
                        {
                            StatusID = j.StatusID,
                            Cost = j.Cost,
                            Price = j.Price,
                            Quantity = j.Quantity == null ? 0 : j.Quantity,
                            OrderDetailID = j.OrderDetailID,
                            LastUpdateDT = j.LastUpdateDT,
                            LastUpdateBy = j.LastUpdateBy,
                            ItemID = j.ItemID,
                            ItemName = j.ItemName,
                            OrderModifierDetails = lstODM,
                            OrderID = j.OrderID,
                            OrderMode = j.OrderMode,
                            Image = j.Image == null ? "" : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.Image.Replace(" ", "%20")


                        });
                    }

                    var _OC = _dsOrdercheckout.Where(x => x.OrderID == i.ID).FirstOrDefault();
                    if (_OC != null)
                    {
                        oc = new OrderCheckoutBLL();
                        oc.AmountPaid = Math.Round((double)_OC.AmountPaid,2);
                        oc.AmountTotal = Math.Round((double)_OC.AmountTotal, 2);
                        oc.CheckoutDate = _OC.CheckoutDate;
                        oc.GrandTotal = Math.Round((double)_OC.GrandTotal, 2);
                        oc.OrderCheckoutID = _OC.OrderCheckoutID;
                        oc.PaymentMode = _OC.PaymentMode;
                        oc.Tax = Math.Round((double)_OC.Tax, 2);
                        oc.ServiceCharges = _OC.ServiceCharges == null ? 0 : Math.Round((double)_OC.ServiceCharges, 2); ;                        
                        oc.OrderID = _OC.OrderID;
                        
                    }
                    else oc = null;

                    var _OCustomer = _dsOrderCustomerData.Where(x => x.OrderID == i.ID).FirstOrDefault();
                    if (_OCustomer != null)
                    {
                        ocustomer = new OrderCustomerBLL();
                        ocustomer.OrderID = _OCustomer.OrderID;
                        ocustomer.Address = _OCustomer.Address;
                        ocustomer.Name = _OCustomer.Name;
                        ocustomer.CustomerOrderID = _OCustomer.CustomerOrderID;
                        ocustomer.Mobile = _OCustomer.Mobile;
                        ocustomer.Email = _OCustomer.Email;
                        ocustomer.StatusID = _OCustomer.StatusID;
                        ocustomer.Latitude = _OCustomer.Latitude;
                        ocustomer.Longitude = _OCustomer.Longitude;
                        ocustomer.AddressNickName = _OCustomer.AddressNickName == null ? "" : _OCustomer.AddressNickName;
                        ocustomer.AddressType = _OCustomer.AddressType == null ? "Other" : _OCustomer.AddressType;
                        ocustomer.OrderID = _OCustomer.OrderID;
                    }
                    else ocustomer = null;

                    string dStatus = _dsOrders.FirstOrDefault().DeliveryStatus;
                    if (dStatus == null || dStatus =="")
                    {
                        dStatus = "0";
                    }                    
                    var OStatus = _dsOrders.FirstOrDefault() == null ? 0 : int.Parse(dStatus);
                    
                    bll.Add(new OrdersBLL
                    {
                        StatusID = i.StatusID,
                        LastUpdatedDate = i.LastUpdatedDate,
                        LastUpdateBy = i.LastUpdateBy,
                        ID = i.ID,
                        CustomerID = i.CustomerID,
                        DeliverUserID = i.DeliverUserID,
                        LocationID = i.LocationID,
                        OrderDate = Convert.ToDateTime(i.OrderCreatedDT).ToString("dd/MM/yyyy"),
                        OrderNo = i.OrderNo,
                        OrderTakerID = i.OrderTakerID,
                        OrderType = i.OrderType,
                        TransactionNo = i.TransactionNo,
                        OrderDetails = lstOD,
                        OrderCheckouts = oc,
                        DeliveryStatus=
                        OStatus == 1 ? "Done"
                        : OStatus == 0 ? "Pending"
                        : OStatus == 2 ? "Ready"
                        : OStatus == 3 ? "Deliver"                       
                        : "Other",                        
                        UserID=i.UserID,
                        CustomerOrders = ocustomer
                    });
                }
                rsp.Orders = bll;
                rsp.status = 1;
                rsp.description = "Success";

                return rsp;
            }
            catch (Exception ex)
            {
                rsp.Orders = bll;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }

        public RspOrdersCustomer GetOrdersAdminV2(int LocationID,string StartDate,string EndDate,string Search)
        {
            var status1 = new OrderStatusChildBLL();
            var status2 = new OrderStatusChildBLL();
            var status3 = new OrderStatusChildBLL();
            var bll = new List<OrdersBLL>();
            var lstOD = new List<OrderDetailBLL>();
            var lstODM = new List<OrderModifierDetailBLL>();
            var oc = new OrderCheckoutBLL();
            var ocustomer = new OrderCustomerBLL();
            var lstOrderStatus = new OrderStatusBLL();
            var rsp = new RspOrdersCustomer();
            try
            {
                var ds = GetAdminOrder_ADO(LocationID, StartDate,EndDate,Search);
                var _dsOrders = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<OrdersBLL>>();
                var _dsorderdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<OrderDetailBLL>>();
                var _dsorderdetailmodifier = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OrderModifiersBLL>>();
                var _dsOrdercheckout = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OrderCheckoutBLL>>();
                var _dsOrderCustomerData = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<OrderCustomerBLL>>();
               
                foreach (var i in _dsOrders.OrderByDescending(x => x.ID))
                {
                    lstOD = new List<OrderDetailBLL>();
                    foreach (var j in _dsorderdetail.Where(x => x.StatusID == 1 && x.OrderID == i.ID))
                    {
                        lstODM = new List<OrderModifierDetailBLL>();
                        foreach (var k in _dsorderdetailmodifier.Where(x => x.StatusID == 201 && x.OrderDetailID == j.OrderDetailID))
                        {
                            lstODM.Add(new OrderModifierDetailBLL
                            {
                                StatusID = k.StatusID,
                                Price = k.Price,
                                ModifierID = k.ModifierID,
                                Cost = k.Cost,
                                OrderDetailID = k.OrderDetailID,
                                OrderModifierDetailID= k.OrderDetailModifierID,
                                Quantity = k.Quantity,
                                ModifierName = k.ModifierName
                            });
                        }

                        lstOD.Add(new OrderDetailBLL
                        {
                            StatusID = j.StatusID,
                            Cost = j.Cost,
                            Price = j.Price,
                            Quantity = j.Quantity == null ? 0 : j.Quantity,
                            OrderDetailID = j.OrderDetailID,
                            LastUpdateDT = j.LastUpdateDT,
                            LastUpdateBy = j.LastUpdateBy,
                            ItemID = j.ItemID,
                            ItemName = j.ItemName,
                            OrderModifierDetails = lstODM,
                            OrderID = j.OrderID,
                            OrderMode = j.OrderMode
                        });
                    }

                    var _OC = _dsOrdercheckout.Where(x => x.OrderID == i.ID).FirstOrDefault();
                    if (_OC != null)
                    {
                        oc = new OrderCheckoutBLL();
                        oc.AmountPaid = _OC.AmountPaid;
                        oc.AmountTotal = _OC.AmountTotal;
                        oc.CheckoutDate = _OC.CheckoutDate == null ?"": DateTime.ParseExact(_OC.CheckoutDate, "MM/dd/yyyy hh:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                        oc.GrandTotal = _OC.GrandTotal;
                        oc.OrderCheckoutID = _OC.OrderCheckoutID;
                        oc.PaymentMode = _OC.PaymentMode;
                        oc.Tax = _OC.Tax;
                        oc.ServiceCharges = _OC.ServiceCharges == null ? 0 : _OC.ServiceCharges;
                        //oc.StatusID = _OC.StatusID;
                        oc.OrderID = _OC.OrderID;
                    }
                    else oc = null;

                    var _OCustomer = _dsOrderCustomerData.Where(x => x.OrderID == i.ID).FirstOrDefault();
                    if (_OCustomer != null)
                    {
                        ocustomer = new OrderCustomerBLL();
                        ocustomer.OrderID = _OCustomer.OrderID;
                        ocustomer.Address = _OCustomer.Address;
                        ocustomer.Name = _OCustomer.Name;
                        ocustomer.CustomerOrderID = _OCustomer.CustomerOrderID;
                        ocustomer.Description = _OCustomer.Description;
                        ocustomer.Mobile = _OCustomer.Mobile;
                        ocustomer.Email = _OCustomer.Email;
                        ocustomer.StatusID = _OCustomer.StatusID;
                        ocustomer.Latitude = _OCustomer.Latitude;
                        ocustomer.Longitude = _OCustomer.Longitude;
                        ocustomer.AddressNickName = _OCustomer.AddressNickName == null ? "" : _OCustomer.AddressNickName;
                        ocustomer.AddressType = _OCustomer.AddressType == null ? "Other" : _OCustomer.AddressType;
                        ocustomer.OrderID = _OCustomer.OrderID;
                    }
                    else ocustomer = null;


                    bll.Add(new OrdersBLL
                    {
                        StatusID = i.StatusID,
                        LastUpdateBy = i.LastUpdateBy,
                        ID = i.ID,
                        CustomerID = i.CustomerID,
                        DeliverUserID = i.DeliverUserID,
                        LocationID = i.LocationID,
                        OrderDate = DateTime.ParseExact(i.OrderCreatedDT, "MM/dd/yyyy hh:mm:ss", null).ToString("dd/MM/yyyy"),
                        OrderNo = i.OrderNo,
                        OrderTakerID = i.OrderTakerID,
                        OrderType = i.OrderType,
                        TransactionNo = i.TransactionNo,
                        OrderDetails = lstOD,
                        OrderCheckouts = oc,
                        DeliveryStatus=i.DeliveryStatus,
                        UserID=i.UserID,
                        CustomerOrders = ocustomer,
                    });
                }
                rsp.Orders = bll;
                rsp.status = 1;
                rsp.description = "Success";


                return rsp;
            }
            catch (Exception ex)
            {
                rsp.Orders = bll;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }
        public RspOrderPunch OrderPunch(OrdersBLL obj)
        {
            RspOrderPunch rsp;
            var UserSetting = DBContext.Users.Where(x => x.ID == obj.UserID).FirstOrDefault();
            try
            {
                if (obj.OrderDetails.Count == 0)
                {
                    rsp = new RspOrderPunch();
                    rsp.status = (int)eStatus.Exception;
                    rsp.description = "Sorry, Your order is failed to punched.";
                    rsp.OrderID = 0;
                }
                else
                {
                    using (var dbContextTransaction = DBContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var currDate = DateTime.UtcNow.AddMinutes(180);
                            var tempCustomerOrders = obj.CustomerOrders;
                            var tempCheckout = obj.OrderCheckouts;


                            obj.CustomerOrders = null;
                            obj.OrderCheckouts = null;
                            Order orders = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<Order>();

                            if (tempCustomerOrders != null)
                            {
                                orders.CustomerOrders1 = new List<CustomerOrder1>();
                                orders.CustomerOrders1.Add(new CustomerOrder1
                                {
                                    Address = tempCustomerOrders.Address,
                                    Description = tempCustomerOrders.Description,
                                    Email = tempCustomerOrders.Email,
                                    Latitude = tempCustomerOrders.Latitude,
                                    Longitude = tempCustomerOrders.Longitude,
                                    Mobile = tempCustomerOrders.Mobile,
                                    Name = tempCustomerOrders.Name,
                                    AddressNickName = tempCustomerOrders.AddressNickName,
                                    AddressType = tempCustomerOrders.AddressType
                                });
                            }
                            if (tempCheckout != null)
                            {
                                orders.OrderCheckouts = new List<OrderCheckout>();
                                orders.OrderCheckouts.Add(new OrderCheckout
                                {
                                    AmountPaid = tempCheckout.AmountPaid,
                                    AmountTotal = tempCheckout.AmountTotal,
                                    CheckoutDate = DateTime.UtcNow.AddMinutes(180),
                                    GrandTotal = tempCheckout.GrandTotal,
                                    ServiceCharges = tempCheckout.ServiceCharges == null ? 0 : tempCheckout.ServiceCharges,
                                    PaymentMode = tempCheckout.PaymentMode,
                                    Tax = tempCheckout.Tax,
                                    OrderStatus = 2,
                                    CustomerID = orders.CustomerID,
                                    LocationID = orders.LocationID
                                });
                            }

                            orders.TransactionNo = DBContext.Orders.Where(x => x.LocationID == obj.LocationID).Max(x => x.TransactionNo);
                            orders.OrderNo = DBContext.Orders.Where(x =>
                            x.LocationID == orders.LocationID
                            && DbFunctions.TruncateTime(x.OrderCreatedDT) == currDate.Date
                            ).Max(x => x.OrderNo);

                            orders.TransactionNo = orders.TransactionNo == null ? 1 : orders.TransactionNo + 1;
                            orders.OrderNo = orders.OrderNo == null ? 1 : orders.OrderNo + 1;
                            orders.OrderCreatedDT = DateTime.UtcNow.AddMinutes(Convert.ToInt32(UserSetting.TimeZoneID)).Date;
                            orders.LastUpdateBy = orders.CustomerID.ToString();
                            orders.LastUpdateDT = DateTime.UtcNow.AddMinutes(Convert.ToInt32(UserSetting.TimeZoneID));
                            orders.StatusID = 2;
                            foreach (var i in orders.OrderDetails)
                            {
                                i.OrderMode = "New";
                                i.StatusID = 1;
                                i.LastUpdateBy = orders.CustomerID.ToString();
                                i.LastUpdateDT = DateTime.UtcNow.AddMinutes(Convert.ToInt32(UserSetting.TimeZoneID));
                                
                                if (i.OrderModifierDetails != null)
                                {
                                    foreach (var j in i.OrderModifierDetails)
                                    {
                                        j.VariantID = j.VariantID==0?null: j.VariantID;
                                        j.ModifierID = j.ModifierID == 0 ? null : j.ModifierID;
                                        j.StatusID = 1;
                                    }
                                }
                                DBContext.sp_DeductStockAdmin(i.ItemId, orders.LocationID, i.Quantity == null ? 0 : i.Quantity, DateTime.UtcNow.AddMinutes(Convert.ToInt32(UserSetting.TimeZoneID)));
                                //here
                            }
                            //foreach (var item in orders.CustomerOrders)
                            //{
                            //    item.StatusID = 1;
                            //    item.LastUpdatedBy = orders.CustomerID.ToString();
                            //    item.LastUpdatedDate = DateTime.UtcNow.AddMinutes(180);
                            //}

                            Order data = DBContext.Orders.Add(orders);
                            DBContext.SaveChanges();

                            dbContextTransaction.Commit();

                            //try
                            //{
                            //    var getTokens = DBContext.PushTokens.Where(x => x.LocationID == obj.LocationID).ToList();
                            //    foreach (var item in getTokens)
                            //    {
                            //        var token = new PushNoticationBLL();
                            //        token.Title = "Lunchbox | New Order";
                            //        token.Message = "You have new order for delivery.";
                            //        token.DeviceID = item.Token;
                            //        PushNotificationAndroid(token);
                            //    }
                            //}
                            //catch (Exception)
                            //{
                            //}

                            rsp = new RspOrderPunch();
                            rsp.status = (int)eStatus.Success;
                            rsp.description = "Your order has been punched successfully.";
                            rsp.OrderID = data.ID;
                            rsp.OrderNo = data.OrderNo;
                        }

                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            rsp = new RspOrderPunch();
                            rsp.status = (int)eStatus.Exception;
                            rsp.description = "Sorry, Your order is failed to punched.";
                            rsp.OrderID = 0;
                            rsp.OrderNo = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rsp = new RspOrderPunch();
                rsp.status = (int)eStatus.Exception;
                rsp.description = "Sorry, Your order is failed to punched.";
                rsp.OrderID = 0;
                rsp.OrderNo = 0;
            }
            return rsp;
        }

        public Rsp CancelOrder(OrdersBLL obj)
        {
            Rsp rsp = new Rsp();

            using (var dbContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    if (obj.ID == 0 || obj.ID == null)
                    {
                        rsp.status = (int)eStatus.Exception;
                        rsp.description = "Order cannot be cancel due to invalid parameter";
                    }
                    else
                    {
                        var currDate = DateTime.UtcNow.AddMinutes(180);

                        Order order = DBContext.Orders.Where(x => x.ID == obj.ID).FirstOrDefault();
                        order.StatusID = 104;
                        order.LastUpdateDT = currDate;
                        DBContext.Orders.AddOrUpdate(order);
                        DBContext.SaveChanges();
                        dbContextTransaction.Commit();

                        rsp.status = (int)eStatus.Success;
                        rsp.description = "Your Orders has been cancelled succesfully";
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    rsp.status = (int)eStatus.Exception;
                    rsp.description = "Sorry! Order cannot be cancelled.";

                }
            }

            return rsp;
        }

        public Rsp UpdateOrderAdmin(int OrderID, int StatusID)
        {
            Rsp rsp = new Rsp();

            using (var dbContextTransaction = DBContext.Database.BeginTransaction())
            {
                try
                {
                    if (OrderID == 0 || StatusID == 0)
                    {
                        rsp.status = (int)eStatus.Exception;
                        rsp.description = "Order cannot be update due to invalid parameter";
                    }
                    else
                    {
                        var currDate = DateTime.UtcNow.AddMinutes(180);

                        Order order = DBContext.Orders.Where(x => x.ID == OrderID).FirstOrDefault();
                        order.DeliveryStatus = StatusID;
                        order.LastUpdateDT = currDate;
                        DBContext.Orders.AddOrUpdate(order);
                        DBContext.SaveChanges();
                        dbContextTransaction.Commit();

                        rsp.status = (int)eStatus.Success;
                        rsp.description = "Order has been updated succesfully";
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    rsp.status = (int)eStatus.Exception;
                    rsp.description = "Sorry! Order cannot be updated.";
                }
            }

            return rsp;
        }
        public DataSet GetCustomerOrder_ADO(int CustomerID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@CustomerID", CustomerID);

                ds = (new DBHelper().GetDatasetFromSP)("sp_GetCustomerOrders_api", p);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAdminOrder_ADO(int LocationID, string StartDate, string EndDate, string Search)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] p = new SqlParameter[4];
                p[0] = new SqlParameter("@LocationID", LocationID);
                p[1] = new SqlParameter("@StartDate", StartDate);
                p[2] = new SqlParameter("@EndDate", EndDate);
                p[3] = new SqlParameter("@Search", Search);

                ds = (new DBHelper().GetDatasetFromSP)("sp_GetAdminOrders_api", p);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
