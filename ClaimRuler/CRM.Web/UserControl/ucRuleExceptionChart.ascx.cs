using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Repository;
using CRM.Data.Views;
using System.Web.UI.DataVisualization.Charting;

namespace CRM.Web.UserControl {
	public partial class ucRuleExceptionChart : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(int clientID) {
			Series series1 = null;
			List<RuleExceptionStatisticsView> statistics = null;

			using (RuleExceptionManager repository = new RuleExceptionManager()) {
				statistics = repository.GetExceptionStatistics(clientID);
			}
			if (statistics != null && statistics.Count > 0) {
				series1 = Chart1.Series[0];

				foreach (RuleExceptionStatisticsView stats in statistics) {
					System.Web.UI.DataVisualization.Charting.DataPoint dataPoint = new System.Web.UI.DataVisualization.Charting.DataPoint();

					dataPoint.AxisLabel = stats.RuleName;

					dataPoint.YValues = new double[] { stats.ExceptionCount };

					// do not show bar for empty values
					dataPoint.IsEmpty = stats.ExceptionCount.Equals(0);
					dataPoint.ToolTip = "Click to drill down";

					series1.Points.Add(dataPoint);


					dataPoint.Url = string.Format("javascript:ruleExceptionDrilldown('{0}','{1}');", stats.RuleID, stats.RuleName);
				}
			}
		}
		
	}
}