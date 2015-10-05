using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace contadores.Classses
{
    public class lecturasTotalGraph
    {
        public List<lecturasIndividual> lecturasIndividual = new List<lecturasIndividual>();
    }
    public class lecturasIndividual
    {
        public List<lecturasIndividualSensorData> lecutrasIndividualSensorData = new List<lecturasIndividualSensorData>();
    }
    public class lecturasIndividualSensorData{
        public List<lecturasData> lecturasData { get; set; }
    }
    public class lecturasData
    {
        public string fecha { get; set; }
        public string litros { get; set; }
        public string edificio { get; set; }
        public string contador { get; set; }
    }
}