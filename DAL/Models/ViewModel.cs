using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    class ViewModel
    {
    }


    public class Rsp
    {
        public string description { get; set; }
        public int status { get; set; }

    }
    public class RspBrandList : Rsp
    {
        public IEnumerable<BrandsBLL> brands { get; set; }
    }
    public class RspAdminLogin : Rsp
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

    }
    public class RspBanner : Rsp
    {
        public List<BannerBLL> Banners { get; set; }
    }
    public class BannerBLL
    {
        public int BannerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> LocationID { get; set; }

    }
    public class AppSettings
    {
        public int UserID { get; set; }
        public int LocationID{ get; set; }
        public string TaxPercent { get; set; }
        public string DeliveryCharges { get; set; }
        public string Currency{ get; set; }
        public string Status { get; set; }

    }
    public class RspOffers
    {
        public int OfferID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string Image { get; set; }
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public string ItemName { get; set; }
        public string Calories { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> StatusID { get; set; }

        public List<LocationsBLL> Locations { get; set; }

    }
    public class RspMenu : Rsp
    {
        public List<CategoryBLL> Categories { get; set; }
    }

    public class RspOrdersCustomer : Rsp
    {
        public IEnumerable<OrdersBLL> Orders { get; set; }
    }
    public class RspOrdersAdmin : Rsp
    {
        public IEnumerable<OrdersBLL> Orders { get; set; }
    }
    public class RspCustomerLogin : Rsp
    {
        public CustomerBLL customer { get; set; }
    }

    public class RspOrderPunch : Rsp
    {
        public int? OrderNo { get; set; }
        public int OrderID { get; set; }
    }
    public class RspCustomerSignup : Rsp
    {
        public int CustomerID { get; set; }
    }
    public class RspCustomerAddress : Rsp
    {
        public int AddressID { get; set; }
    }
    public class RspCustomerPayment : Rsp
    {
        public int PaymentID { get; set; }
    }
    public class BrandsBLL
    {
        public int BrandID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyURl { get; set; }
        public string Address { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Currency { get; set; }
        public float Tax { get; set; }
        public Nullable<long> BusinessKey { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }

        public List<LocationsBLL> Locations { get; set; }
    }

    public class LocationsBLL
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public int LicenseID { get; set; }
        public Nullable<bool> DeliveryServices { get; set; }
        public Nullable<double> DeliveryCharges { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<double> MinOrderAmount { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string ImageURL { get; set; }
        public Nullable<int> BrandID { get; set; }

    }

    public class CategoryBLL
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> LocationID { get; set; }

        public List<SubCategoryBLL> Subcategories { get; set; }
    }
    public class SubCategoryBLL
    {
        public int SubCategoryID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> StatusID { get; set; }

        public List<ItemBLL> Items { get; set; }
    }
    public class ItemBLL
    {
        public int ID { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public string Name { get; set; }
        public string NameOnReceipt { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public string ItemType { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string[] ItemImages { get; set; }
        public int? DisplayOrder { get; set; }

        public bool? IsFeatured { get; set; }
        public List<ModifierBLL> Modifiers { get; set; }
        public List<VariantsBLL> Variants { get; set; }
    }
    public class VariantsBLL
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> StatusID { get; set; }

    }
    public class ItemModifierMappingBLL
    {
        public int ID { get; set; }
        public int ItemID { get; set; }

    }
    public class ModifierBLL
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> StatusID { get; set; }
    }

    public class CustomerBLL
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> UserID { get; set; }
        public List<CustomerAddressBLL> Addresses { get; set; }
        public List<CustomerPaymentBLL> Cards { get; set; }
    }

    public class CustomerDetailBLL
    {
        public int CustomerDetailID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ZipCode { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    }
    public class CustomerAddressBLL
    {

        public int? CustomerAddressID { get; set; }
        public string Address { get; set; }
        public string NickName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string StreetName { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string Country { get; set; }
        public string ContactNo { get; set; }
    }
    public class CustomerPaymentBLL
    {
        public int PaymentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CardExpire { get; set; }
        public string CVV { get; set; }
        public string CardTitle { get; set; }
        public Nullable<int> StatusID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> BrandID { get; set; }
    }

    public class TokenBLL
    {
        public int Device { get; set; }
        public int TokenID { get; set; }
        public string Token { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> StatusID { get; set; }

    }
    public class OrdersBLL
    {
        public int ID { get; set; }
        public string DeliveryStatus { get; set; }
        public int UserID{ get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> TransactionNo { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string OrderType { get; set; }
        public string OrderCreatedDT { get; set; }
        public string OrderDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> OrderTakerID { get; set; }
        public Nullable<int> DeliverUserID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LocationID { get; set; }
        public OrderCheckoutBLL OrderCheckouts { get; set; }
        public OrderCustomerBLL CustomerOrders { get; set; }
        public List<OrderDetailBLL> OrderDetails { get; set; }
    }

    public class OrderStatusChildBLL
    {
        public string Value { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
    }
    public class OrderDetailBLL
    {
        public int ID { get; set; }
        public int OrderDetailID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public string OrderMode { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
        public List<OrderModifierDetailBLL> OrderModifierDetails { get; set; }

        public string Image { get; set; }
    }
    // Add By Ammad 
    public class OrderModifierDetailBLL
    {

        public string Name { get; set; }
        public int OrderModifierDetailID { get; set; }
        public Nullable<int> OrderDetailID { get; set; }
        public Nullable<int> ModifierID { get; set; }
        public Nullable<int> VariantID { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> Price { get; set; }
        public string Type { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<double> Quantity { get; set; }

        //Modifer and Variant Details
        public string ModifierName { get; set; }
        public string VariantName { get; set; }
    }
    public class OrderModifiersBLL
    {
        public int OrderDetailModifierID { get; set; }
        public Nullable<int> OrderDetailID { get; set; }
        public Nullable<int> ModifierID { get; set; }
        public string ModifierName { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
    }
    public class OrderCheckoutBLL
    {
        public int OrderCheckoutID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public Nullable<double> AmountPaid { get; set; }
        public Nullable<double> AmountTotal { get; set; }
        public Nullable<double> ServiceCharges { get; set; }
        public Nullable<double> GrandTotal { get; set; }
        public Nullable<double> Tax { get; set; }
        public string CheckoutDate { get; set; }
        //public Nullable<int> StatusID { get; set; }
        //public Nullable<int> OrderStatus { get; set; }
    }
    public class OrderCustomerBLL
    {
        public int CustomerOrderID { get; set; }
        public string Name { get; set; }
        public string Email{ get; set; }
        public string Address{ get; set; }
        public string Description { get; set; }
        public string Mobile { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LocationURL { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string AddressNickName { get; set; }
        public string AddressType { get; set; }
    }
    public class PushNoticationBLL
    {
        public string DeviceID { get; set; }
        public string Type { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
    }
    public class OrderStatusBLL
    {
        public OrderStatusChildBLL OrderConfirmed { get; set; }
        public OrderStatusChildBLL FoodPrepared { get; set; }
        public OrderStatusChildBLL DeliveryinProgress { get; set; }
    }
   
}
