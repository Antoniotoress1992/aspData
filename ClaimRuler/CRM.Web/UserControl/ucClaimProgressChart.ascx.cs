using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Core;
using CRM.Repository;

using System.Web.UI.DataVisualization.Charting;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucClaimProgressChart : System.Web.UI.UserControl {

        int roleID = 0;
        int clientID = 0;
        int userID = 0;
        public System.Web.UI.HtmlControls.HtmlInputHidden hiddenvalue;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            hiddenvalue = hiddenSearchClick;
        }

		protected void Page_Load(object sender, EventArgs e) {
            /*--- Chetu Code ----*/
            roleID = SessionHelper.getUserRoleId();
            clientID = SessionHelper.getClientId();
            userID = SessionHelper.getUserId();
            if(!IsPostBack)
            {
            BindCarrier();
            BindAdjuster();
            }
            /*--- Chetu Code ----*/
		}

        public void bindData(int clientID, int userID)
        {
            Series series1 = null;
            List<ProgressStatisticsView> claimStatistics = null;

            using (ProgressStatusManager repository = new ProgressStatusManager())
            {
                claimStatistics = repository.getProgressStatistics(clientID, userID);
            }

            if (claimStatistics != null && claimStatistics.Count > 0)
            {
                series1 = Chart1.Series[0];

                foreach (ProgressStatisticsView stats in claimStatistics)
                {
                    System.Web.UI.DataVisualization.Charting.DataPoint dataPoint = new System.Web.UI.DataVisualization.Charting.DataPoint();

                    dataPoint.AxisLabel = stats.ProgressDescription;

                    dataPoint.YValues = new double[] { stats.claimCount };

                    // do not show bar for empty values
                    dataPoint.IsEmpty = stats.claimCount.Equals(0);
                    dataPoint.ToolTip = "Click to drill down";

                    series1.Points.Add(dataPoint);

                    dataPoint.Url = string.Format("javascript:progressDrilldown('{0}','{1}');", stats.ProgressStatusID, stats.ProgressDescription);

                }
            }
        }


		public void bindData(int clientID) {
			Series series1 = null;
			List<ProgressStatisticsView> claimStatistics = null;

			using (ProgressStatusManager repository = new ProgressStatusManager()) {
				claimStatistics = repository.getProgressStatistics(clientID);
			}
			if (claimStatistics != null && claimStatistics.Count > 0) {
				series1 = Chart1.Series[0];

				foreach (ProgressStatisticsView stats in claimStatistics) {
					System.Web.UI.DataVisualization.Charting.DataPoint dataPoint = new System.Web.UI.DataVisualization.Charting.DataPoint();

					dataPoint.AxisLabel = stats.ProgressDescription;

					dataPoint.YValues = new double[] { stats.claimCount };
					//dataPoint.Url = "~/Protected/ClaimEdit.aspx";

					// do not show bar for empty values
					dataPoint.IsEmpty = stats.claimCount.Equals(0);
					dataPoint.ToolTip = "Click to drill down";

					series1.Points.Add(dataPoint);


					//dataPoint.MapAreaAttributes = string.Format("onclick=\"alert('{0}')\";", stats.ProgressStatusID);
					dataPoint.Url = string.Format("javascript:progressDrilldown('{0}','{1}');", stats.ProgressStatusID, stats.ProgressDescription);
					//Chart1.Series[0].Points.AddXY(stats.ProgressDescription, stats.claimCount);
				}
				//series1.Sort(PointSortOrder.Ascending, "AxisLabel");
				//series1.MapAreaAttributes = 

			}
		}

        /*--- Chetu Code ----*/
        public void bindDataFilter(int clientID, int userID, int adjusterId, int carrierId)
        {
            Series series1 = null;
            List<ProgressStatisticsView> claimStatistics = null;

            using (ProgressStatusManager repository = new ProgressStatusManager())
            {
                claimStatistics = repository.getProgressStatisticsFilter(clientID, userID,adjusterId,carrierId);
            }

            if (claimStatistics != null && claimStatistics.Count > 0)
            {
                series1 = Chart1.Series[0];

                foreach (ProgressStatisticsView stats in claimStatistics)
                {
                    System.Web.UI.DataVisualization.Charting.DataPoint dataPoint = new System.Web.UI.DataVisualization.Charting.DataPoint();

                    dataPoint.AxisLabel = stats.ProgressDescription;

                    dataPoint.YValues = new double[] { stats.claimCount };

                    // do not show bar for empty values
                    dataPoint.IsEmpty = stats.claimCount.Equals(0);
                    dataPoint.ToolTip = "Click to drill down";

                    series1.Points.Add(dataPoint);

                    dataPoint.Url = string.Format("javascript:progressDrilldown('{0}','{1}');", stats.ProgressStatusID, stats.ProgressDescription);

                }
            }
        }



        public void bindDataFilter(int clientID,int adjusterId,int carrierId) {
			Series series1 = null;
			List<ProgressStatisticsView> claimStatistics = null;

			using (ProgressStatusManager repository = new ProgressStatusManager()) {
                claimStatistics = repository.getProgressStatisticsFilter(clientID, adjusterId, carrierId);
			}
			if (claimStatistics != null && claimStatistics.Count > 0) {
				series1 = Chart1.Series[0];
                series1.Points.Clear();

				foreach (ProgressStatisticsView stats in claimStatistics) {
					System.Web.UI.DataVisualization.Charting.DataPoint dataPoint = new System.Web.UI.DataVisualization.Charting.DataPoint();

					dataPoint.AxisLabel = stats.ProgressDescription;

					dataPoint.YValues = new double[] { stats.claimCount };
					//dataPoint.Url = "~/Protected/ClaimEdit.aspx";

					// do not show bar for empty values
					dataPoint.IsEmpty = stats.claimCount.Equals(0);
					dataPoint.ToolTip = "Click to drill down";

					series1.Points.Add(dataPoint);


					//dataPoint.MapAreaAttributes = string.Format("onclick=\"alert('{0}')\";", stats.ProgressStatusID);
					dataPoint.Url = string.Format("javascript:progressDrilldown('{0}','{1}');", stats.ProgressStatusID, stats.ProgressDescription);
					//Chart1.Series[0].Points.AddXY(stats.ProgressDescription, stats.claimCount);
				}
				//series1.Sort(PointSortOrder.Ascending, "AxisLabel");
				//series1.MapAreaAttributes = 

			}
		}

        /*--- Chetu Code ----*/

        /*--- Chetu Code ----*/
        protected void lbtrnSearchPanel_Click(object sender, EventArgs e)
        {
            hiddenSearchClick.Value = "0";
            int adusterId=Convert.ToInt32(ddlAdjuster.SelectedValue);
            int carrierId = Convert.ToInt32(ddlCarrier.SelectedValue);
           bindDataWithFilter(adusterId, carrierId);
        }
        /*--- Chetu Code ----*/
        /// <summary>
        /// function used for bind carrier dropdownlist
        /// develop by chetu team
        /// </summary>
        private void BindCarrier()
        {
            List<Carrier> listCarrier = null;
            using (ProgressStatusManager psm = new ProgressStatusManager())
            {
                listCarrier = psm.GetCarrierData(clientID);
            }
            ddlCarrier.DataSource = listCarrier;
            ddlCarrier.DataTextField = "CarrierName";
            ddlCarrier.DataValueField = "CarrierID";
            ddlCarrier.DataBind();
            ddlCarrier.Items.Insert(0,new ListItem("All","0"));
        }

        /// <summary>
        /// function used for bind carrier dropdownlist
        /// develop by chetu team
        /// </summary>
        private void BindAdjuster()
        {
            List<AdjusterMaster> listAdjuster=null;
            using (ProgressStatusManager psm = new ProgressStatusManager())
            {
                listAdjuster = psm.GetAdjsuterData(clientID);
            }
            ddlAdjuster.DataSource = listAdjuster;
            ddlAdjuster.DataTextField = "AdjusterName";
            ddlAdjuster.DataValueField = "AdjusterId";
            ddlAdjuster.DataBind();
            ddlAdjuster.Items.Insert(0, new ListItem("All", "0"));

        }

        /*--- Chetu Code ----*/
        private void bindDataWithFilter(int adusterId, int carrierId)
        {
            switch (roleID)
            {
                case (int)UserRole.Administrator:
                    break;
                case (int)UserRole.Client:
                case (int)UserRole.SiteAdministrator:
                    bindDataFilter(clientID, adusterId, carrierId);                    
                    break;
                default:
                    bindDataFilter(clientID, userID, adusterId, carrierId);                   
                    break;
            }

        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }
        /*--- Chetu Code ----*/
	}
}