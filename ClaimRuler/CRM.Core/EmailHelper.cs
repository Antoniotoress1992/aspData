using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

using System.Net;
using System.Net.Mail;

namespace CRM.Core {
	static public class EmailHelper {
		static public void sendEmail(string fromEmail, string[] recipients, string[] cc, string subject,
			string bodyText, string[] attachments, string host, int port, string userName, string password) {

			System.Net.Mail.Attachment attachment = null;

			using (MailMessage message = new MailMessage()) {

				// add recipients
				for (int i = 0; i < recipients.Length; i++) {
					message.To.Add(new MailAddress(recipients[i]));
				}

				message.From = new MailAddress(fromEmail);
				message.Body = bodyText;
				message.IsBodyHtml = true;
				message.Subject = subject;

				if (cc != null) {
					foreach (string c in cc) {
						message.CC.Add(new MailAddress(c));
					}
				}

				if (attachments != null && attachments.Length > 0) {
					foreach (string filepath in attachments) {
						attachment = new System.Net.Mail.Attachment(filepath);

						message.Attachments.Add(attachment);
					}
				}



				using (SmtpClient mailClient = new SmtpClient()) {
					mailClient.Host = host;

					mailClient.Port = port;

					mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

					mailClient.UseDefaultCredentials = false;

					mailClient.EnableSsl = true;

					//mailClient.Credentials = new NetworkCredential("tortega@itstrategiesgroup.com", "Angelo2000");
					mailClient.Credentials = new NetworkCredential(userName, password);

					mailClient.Send(message);
				}
			}
		}

		//static public void sendEmail(string fromEmail, string[] recipients, string subject, string bodyText) {
		//	using (MailMessage message = new MailMessage()) {

		//		// add recipients
		//		for (int i = 0; i < recipients.Length; i++) {
		//			message.To.Add(new MailAddress(recipients[i]));
		//		}

		//		message.From = new MailAddress(fromEmail);
		//		message.Body = bodyText;
		//		message.IsBodyHtml = true;
		//		message.Subject = subject;

		//		using (SmtpClient mailClient = new SmtpClient()) {

		//			mailClient.EnableSsl = true;

		//			mailClient.Send(message);
		//		}
		//	}
		//}

		//static public void sendEmail(string fromEmail, string[] recipients, string[] cc, string subject, string bodyText, string[] attachments) {
		//	System.Net.Mail.Attachment attachment = null;

		//	using (MailMessage message = new MailMessage()) {

		//		// add recipients
		//		for (int i = 0; i < recipients.Length; i++) {
		//			message.To.Add(new MailAddress(recipients[i]));
		//		}

		//		message.From = new MailAddress(fromEmail);
		//		message.Body = bodyText;
		//		message.IsBodyHtml = true;
		//		message.Subject = subject;

		//		if (cc != null) {
		//			foreach (string c in cc) {
		//				message.CC.Add(new MailAddress(c));
		//			}
		//		}

		//		if (attachments != null && attachments.Length > 0) {
		//			foreach (string filepath in attachments) {
		//				attachment = new System.Net.Mail.Attachment(filepath);

		//				message.Attachments.Add(attachment);
		//			}
		//		}



		//		using (SmtpClient mailClient = new SmtpClient()) {
		//			mailClient.Host = "smtp.gmail.com";

		//			mailClient.Port = 587;

		//			mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

		//			mailClient.UseDefaultCredentials = false;

		//			mailClient.EnableSsl = true;

		//			mailClient.Credentials = new NetworkCredential("tortega@itstrategiesgroup.com", "Angelo2000");

		//			mailClient.Send(message);
		//		}
		//	}
		//}

		static public void emailError(Exception ex) {
			string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
			string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			string[] recipients = null;
			string subject = "Claim Ruler Error";

			System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

			// system administrator email
			recipients = new string[] { ConfigurationManager.AppSettings["userID"].ToString() };

			if (HttpContext.Current.Session != null) {

				// get logged username
				string userName = HttpContext.Current.Session["UserName"] == null ? "" : HttpContext.Current.Session["UserName"].ToString();

				// get client id user belongs to
				string clientID = HttpContext.Current.Session["ClientId"] == null ? "None" : HttpContext.Current.Session["ClientId"].ToString();

				// build email body
				sb.Append("<br/><br/><b>URL: </b>" + HttpContext.Current.Request.Url);

				sb.Append("<br/><br/><b>Client ID: </b>" + clientID);

				sb.Append("<br/><br/><b>User Name: </b>" + userName);

				sb.Append("<br/><br/><b>Lead ID: </b>" + string.Format("{0}", HttpContext.Current.Session["LeadIds"] ?? 0));

				sb.Append("<br/><br/><b>Policy ID: </b>" + string.Format("{0}", HttpContext.Current.Session["policyID"] ?? 0));

				sb.Append("<br/><br/><b>Claim ID: </b>" + string.Format("{0}", HttpContext.Current.Session["ClaimID"] ?? 0));

				sb.Append("<br/><br/><b>Source: </b>" + ex.Source);

				sb.Append("<br/><br/><b>Exception: </b>" + ex.Message);

				sb.Append("<br/><br/><b>Method: </b>" + trace.GetFrame(0).GetMethod().ReflectedType.FullName);
				sb.Append("<br/><br/><b>Line #: </b>" + trace.GetFrame(0).GetFileLineNumber().ToString());

				sb.Append("<br/><br/><b>Stack Trace:</b><br/>" + ex.StackTrace);

				string innerExceptionMessage = ex.InnerException == null ? "None" : ex.InnerException.Message;

				sb.Append("<br/><br/><b>Inner Exception:</b><br/>" + innerExceptionMessage);

				string bodyText = sb.ToString();

				Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, bodyText, null, crsupportEmail, crsupportEmailPassword);
			}
			else {
				sb.Append("<br/><br/><b>URL: </b>" + HttpContext.Current.Request.Url);

				sb.Append("<br/><br/><b>Source: </b>" + ex.Source);

				sb.Append("<br/><br/><b>Exception: </b>" + ex.Message);

				sb.Append("<br/><br/><b>Method: </b>" + trace.GetFrame(0).GetMethod().ReflectedType.FullName);

				sb.Append("<br/><br/><b>Line #: </b>" + trace.GetFrame(0).GetFileLineNumber().ToString());


				sb.Append("<br/><br/><b>Stack Trace:</b><br/>" + ex.StackTrace);

				string innerExceptionMessage = ex.InnerException == null ? "None" : ex.InnerException.Message;

				sb.Append("<br/><br/><b>Inner Exception:</b><br/>" + innerExceptionMessage);

				string bodyText = sb.ToString();

				Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, bodyText, null, crsupportEmail, crsupportEmailPassword);
			}
		}

		static public void emailUserCredentials(Data.Entities.SecUser user) {
			string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
			string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
			string siteURL = ConfigurationManager.AppSettings["siteURL"].ToString();
			StringBuilder emailBody = null;

			if (user != null && !string.IsNullOrEmpty(user.Email)) {
				string[] recipients = new string[] { user.Email };

				// get new user password to show in email
				string password = SecurityManager.Decrypt(user.Password);

				string subject = "ClaimRuler Access Account";

				emailBody = new StringBuilder();
				// .containerBox
				emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

				// header
				emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

				// .paneContentInner
				emailBody.Append("<div style=\"margin: 20px;\">");
				emailBody.Append("<p>Congratulations, " + user.FirstName + "!</p>");
				emailBody.Append("<p>You have been granted access to Claim Ruler - Industrial Strength Property Claim Management Software.</p>");
				emailBody.Append("<p>User name: <b>" + user.UserName + "</b></p>");
				emailBody.Append("<p>Password: <b>" + password + "</b></p>");
				emailBody.Append("<br><br>Please click on the link below to sign in.<br>");
				emailBody.Append(string.Format("<a target='_blank' href='{0}/login.aspx'>Claim Ruler - Industrial Strength Property Claim Management Software</a>", siteURL));


				emailBody.Append("</div>");	// paneContentInner
				emailBody.Append("</div>");	// containerBox


				Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);
			}
		}

		static public void emailUserPasswordReset(Data.Entities.SecUser user) {
			string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
			string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
			string siteURL = ConfigurationManager.AppSettings["siteURL"].ToString();
			StringBuilder emailBody = null;

			if (user != null && !string.IsNullOrEmpty(user.Email)) {
				string[] recipients = new string[] { user.Email };

				// get new user password to show in email
				string password = SecurityManager.Decrypt(user.Password);

				string subject = "ClaimRuler Password Reset";

				emailBody = new StringBuilder();
				// .containerBox
				emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

				// header
				emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

				// .paneContentInner
				emailBody.Append("<div style=\"margin: 20px;\">");
				emailBody.Append("<p>Dear, " + user.FirstName + "!</p>");
				emailBody.Append("<p>Your ClaimRuler password has been reset.</p>");
				emailBody.Append("<p>User name: <b>" + user.UserName + "</b></p>");
				emailBody.Append("<p>New Password: <b>" + password + "</b></p>");
				emailBody.Append("<br><br>Please click on the link below to sign in.<br>");
				emailBody.Append(string.Format("<a target='_blank' href='{0}/login.aspx'>Claim Ruler - Industrial Strength Property Claim Management Software</a>", siteURL));


				emailBody.Append("</div>");	// paneContentInner
				emailBody.Append("</div>");	// containerBox


				Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);
			}
		}

		/// <summary>
		/// Uses smtp.gmail.com as default SMTP server with SSL for sending emails
		/// </summary>
		/// <param name="fromEmail"></param>
		/// <param name="recipients"></param>
		/// <param name="cc"></param>
		/// <param name="subject"></param>
		/// <param name="bodyText"></param>
		/// <param name="attachments"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		static public void sendEmail(string fromEmail, string[] recipients, string[] cc, string subject, string bodyText, string[] attachments, string userName, string password) {
			System.Net.Mail.Attachment attachment = null;

			using (MailMessage message = new MailMessage()) {

				// add recipients
				for (int i = 0; i < recipients.Length; i++) {
					message.To.Add(new MailAddress(recipients[i]));
				}

				message.From = new MailAddress(fromEmail);
				message.Body = bodyText;
				message.IsBodyHtml = true;
				message.Subject = subject;

				if (cc != null) {
					foreach (string c in cc) {
						message.CC.Add(new MailAddress(c));
					}
				}

				if (attachments != null && attachments.Length > 0) {
					foreach (string filepath in attachments) {
						attachment = new System.Net.Mail.Attachment(filepath);

						message.Attachments.Add(attachment);
					}
				}



				using (SmtpClient mailClient = new SmtpClient()) {
					mailClient.Host = ConfigurationManager.AppSettings["smtpHost"].ToString();

					mailClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);

					mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

					// using connection credential instead
					mailClient.UseDefaultCredentials = false;

					mailClient.EnableSsl = true;

					mailClient.Credentials = new NetworkCredential(userName, password);

					mailClient.Send(message);
				}
			}
		}

		static public void sendEmail(string fromEmail, string[] recipients, string[] cc, string subject, string bodyText, string[] attachments, string host, int port, string userName, string password, bool isSSL) {
			System.Net.Mail.Attachment attachment = null;

			using (MailMessage message = new MailMessage()) {

				// add recipients
				for (int i = 0; i < recipients.Length; i++) {
					if (!string.IsNullOrEmpty(recipients[i]))
						message.To.Add(new MailAddress(recipients[i]));
				}

				message.From = new MailAddress(fromEmail);
				message.Body = bodyText;
				message.IsBodyHtml = true;
				message.Subject = subject;
              

				if (cc != null) {
					foreach (string c in cc) {
						message.CC.Add(new MailAddress(c));
					}
				}

				if (attachments != null && attachments.Length > 0) {
					foreach (string filepath in attachments) {
						attachment = new System.Net.Mail.Attachment(filepath);

						message.Attachments.Add(attachment);
					}
				}



				using (SmtpClient mailClient = new SmtpClient()) {
					mailClient.Host = host;

					mailClient.Port = port;

					mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

					// using connection credential instead
					mailClient.UseDefaultCredentials = false;

					//mailClient.EnableSsl = true;
					mailClient.EnableSsl = isSSL;

					mailClient.Credentials = new NetworkCredential(userName, password);

					mailClient.Send(message);
				}
			}
		}

	}
}