//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.DBEntities
{
    using System;
    
    public partial class sp_SalesSummaryMultiLoc_Result
    {
        public Nullable<double> GrossSales { get; set; }
        public Nullable<double> TotalDiscount { get; set; }
        public Nullable<double> ServiceCharges { get; set; }
        public Nullable<decimal> TaxPercent { get; set; }
        public Nullable<double> TaxAmount { get; set; }
        public Nullable<double> RefundAmount { get; set; }
        public Nullable<double> NetSales { get; set; }
        public Nullable<int> CashOrder { get; set; }
        public Nullable<double> CashOrderAmount { get; set; }
        public Nullable<int> CreditOrder { get; set; }
        public Nullable<double> CreditOrderAmount { get; set; }
        public Nullable<int> CardOrder { get; set; }
        public Nullable<double> CardOrderAmount { get; set; }
        public Nullable<int> MultiOrder { get; set; }
        public Nullable<int> VoidOrder { get; set; }
        public Nullable<int> RefundOrder { get; set; }
        public Nullable<int> TotalOrders { get; set; }
        public Nullable<double> Checkout { get; set; }
        public Nullable<int> CheckoutOrders { get; set; }
        public Nullable<double> Delivery { get; set; }
        public Nullable<int> DeliveryOrders { get; set; }
    }
}
