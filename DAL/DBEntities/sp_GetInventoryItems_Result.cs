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
    
    public partial class sp_GetInventoryItems_Result
    {
        public string Name { get; set; }
        public int InventoryID { get; set; }
        public int ID { get; set; }
        public string PurchasingUnit { get; set; }
        public Nullable<double> CostPerUnit { get; set; }
        public Nullable<double> CurrentStockLevel { get; set; }
    }
}
