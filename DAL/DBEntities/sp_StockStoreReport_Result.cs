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
    
    public partial class sp_StockStoreReport_Result
    {
        public string Name { get; set; }
        public int ItemID { get; set; }
        public double MinimumStock { get; set; }
        public double CurrentStock { get; set; }
        public double StockSold { get; set; }
        public double OpeningStock { get; set; }
        public double ReturnStock { get; set; }
    }
}