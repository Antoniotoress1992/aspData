using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace contadores.Classses
{
    public class lecturasPrint
    {
        [Key]
        public string edificio { get; set; }
        public string contador { get; set; }
        public string litros { get; set; }  
        public string fecha { get; set; }
        
    }
}