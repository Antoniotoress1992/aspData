using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgniteUI.SamplesBrowser.Models.Financial
{
    public class FinancialDataCollection : List<FinancialDataPoint>
    {
        public FinancialDataCollection()
        {
            this.Add(new FinancialDataPoint { Spending = 20, Budget = 60, Label = "Administration", });
            this.Add(new FinancialDataPoint { Spending = 80, Budget = 40, Label = "Sales", });
            this.Add(new FinancialDataPoint { Spending = 20, Budget = 60, Label = "IT", });
            this.Add(new FinancialDataPoint { Spending = 80, Budget = 40, Label = "Marketing", });
            this.Add(new FinancialDataPoint { Spending = 20, Budget = 60, Label = "Development", });
            this.Add(new FinancialDataPoint { Spending = 60, Budget = 20, Label = "Customer Support", });
        }

    }

    public class FinancialDataPoint
    {
        public string Label { get; set; }
        public double Spending { get; set; }
        public double Budget { get; set; }


        public string ToolTip { get { return String.Format("{0}, Spending {1}, Budget {2}", Label, Spending, Budget); } }

    }
}