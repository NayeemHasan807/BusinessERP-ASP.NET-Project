//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessERP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RawMaterialUsesLog
    {
        public int LogId { get; set; }
        public int MaterialId { get; set; }
        public int UsedQuantity { get; set; }
        public double UnitPrice { get; set; }
        public System.DateTime Date { get; set; }
        public double ManufacturingCost { get; set; }
    
        public virtual RawMaterial RawMaterial { get; set; }
    }
}
