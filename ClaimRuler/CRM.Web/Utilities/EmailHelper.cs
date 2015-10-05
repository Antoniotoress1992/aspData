using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Mail;

namespace CRM.Web.Utilities {
	static public class EmailHelper {

		static public void sendEmail(string fromEmail, string[] recipients, string subject, string bodyText) {
			using (MailMessage message = new MailMessage()) {

				// add recipients
				for (int i = 0; i < recipients.Length; i++) {
					message.To.Add(new MailAddress(recipients[i]));
				}

				message.From = new MailAddress(fromEmail);
				message.Body = bodyText;
				message.IsBodyHtml = true;
				message.Subject = subject;

				using (SmtpClient mailClient = new SmtpClient()) {

					mailClient.EnableSsl = true;

					mailClient.Send(message);
				}
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
					mailClient.Host = "smtp.gmail.com";

					mailClient.Port = 587;

					mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

					// using connection credential instead
					mailClient.UseDefaultCredentials = false;

					mailClient.EnableSsl = true;

					//mailClient.Credentials = new NetworkCredential("tortega@itstrategiesgroup.com", "Angelo2000");
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