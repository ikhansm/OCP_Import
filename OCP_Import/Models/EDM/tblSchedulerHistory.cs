//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OCP_Import.Models.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSchedulerHistory
    {
        public long Id { get; set; }
        public int SellerId { get; set; }
        public System.DateTime Run_at { get; set; }
    
        public virtual tblSeller tblSeller { get; set; }
    }
}
