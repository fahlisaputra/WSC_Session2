//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSC_Session2
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChangedPart
    {
        public long ID { get; set; }
        public long EmergencyMaintenanceID { get; set; }
        public long PartID { get; set; }
        public decimal Amount { get; set; }
    
        public virtual EmergencyMaintenance EmergencyMaintenance { get; set; }
        public virtual Part Part { get; set; }
    }
}
