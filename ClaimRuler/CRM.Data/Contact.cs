using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public partial class Contact
    {
        public string fullName
        {
            get
            {
                return this.ContactName == null ? this.FirstName + " " + this.LastName : this.ContactName;
            }
        }
        public string contactName
        {
            get
            {
                return this.ContactName == null ? this.FirstName + " " + this.LastName : this.ContactName;
            }
        }
    }
}
