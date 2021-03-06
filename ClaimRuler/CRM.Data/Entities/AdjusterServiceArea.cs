//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdjusterServiceArea
    {
        public int ID { get; set; }
        public Nullable<int> AdjusterID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<System.DateTime> LicenseEffectiveDate { get; set; }
        public Nullable<System.DateTime> LicenseExpirationDate { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public Nullable<int> AppointmentTypeID { get; set; }
    
        public virtual AdjusterLicenseAppointmentType AdjusterLicenseAppointmentType { get; set; }
        public virtual AdjusterMaster AdjusterMaster { get; set; }
        public virtual StateMaster StateMaster { get; set; }
    }
}
