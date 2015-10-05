using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using contadores.Models;

namespace contadores.Classses
{
    public class lecturasTotal
    {
        public List<lecturas> lecturasTableData { get; set;}
        public string startdate { get; set; }
        public string endDate { get; set; }
        public lecturasCI lecturasCi { get; set; }
        public lecturasRI lecturasRI { get; set; }
        public lecturasRED lecturasRED { get; set; }
    }

    public class lecturasCI
    {
        
        public List<lecturas> lecturasCiChartData { get; set; }
        public string maxValue { get; set; }
    }

    public class lecturasRI
    {
       
        public List<lecturas> lecturasCiChartData { get; set; }
        public string maxValue { get; set; }
    }

    public class lecturasRED
    {
        
        public List<lecturas> lecturasCiChartData { get; set; }
        public string maxValue { get; set; }
    }

   
}
