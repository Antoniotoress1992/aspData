using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class Comment : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {			
		}

		protected void btnSave_click(object sender, EventArgs e) {
            CRM.Data.Entities.LeadComment comment = null;
			string policyType = null;

			if (Page.Request.Params["t"] != null) {
				policyType = Page.Request.Params["t"].ToString();
								
				int policyTypeID = (int)Enum.Parse(typeof(PolicyType), policyType);

                comment = new CRM.Data.Entities.LeadComment();

				comment.CommentText = txtComment.Text.Trim();
				comment.InsertBy = Convert.ToInt32(Session["UserId"].ToString());
				comment.InsertDate = DateTime.Now;
				comment.LeadId = Convert.ToInt32(Session["LeadIds"].ToString());
				comment.PolicyType = policyTypeID;
				comment.Status = 1;
				comment.UserId = comment.InsertBy;

				LeadCommentManager.Save(comment);

				string js = "<script type='text/javascript'>closeRefresh();</script>";

				ScriptManager.RegisterStartupScript(this, typeof(Page), "commentk", js, false);
			}
		}
	}
}