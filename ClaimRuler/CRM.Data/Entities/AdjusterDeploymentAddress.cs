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
    
    public partial class AdjusterDeploymentAddress
    {
        public AdjusterDeploymentAddress()
        {
            this.AdjusterDeploymentEvent = new HashSet<AdjusterDeploymentEvent>();
        }
    
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string DeploymentAddress { get; set; }
        public string DeploymentAddress2 { get; set; }
        public string City { get; set; }
        public Nullable<int> States { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ICollection<AdjusterDeploymentEvent> AdjusterDeploymentEvent { get; set; }
        public virtual SecUser SecUser { get; set; }
    }
}
