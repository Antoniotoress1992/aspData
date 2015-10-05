using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using CRM.Data;

using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

using Atom.Core;
using Atom.Core.Collections;

using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;


//using EAGetMail;
using ImapX;


namespace CRM.Web.Utilities {
	static public class GCalendarHelper {
		// GMAIL URL for Atom Email Reading
		private static readonly Uri _gmailServerUri = new Uri("https://mail.google.com/mail/feed/atom");

		static public void export(string email, string password, List<LeadTask> tasks) {
			CalendarService myService = new CalendarService("exportToGCalendar");
			myService.setUserCredentials(email, password);

			foreach (LeadTask task in tasks) {
				EventEntry entry = new EventEntry();

				// Set the title and content of the entry.
				entry.Title.Text = task.text;
				entry.Content.Content = task.details;


				When eventTime = new When((DateTime)task.start_date, (DateTime)task.end_date);
				entry.Times.Add(eventTime);

				Uri postUri = new Uri("https://www.google.com/calendar/feeds/default/private/full");

				// Send the request and receive the response:
				Google.GData.Client.AtomEntry insertedEntry = myService.Insert(postUri, entry);
			}
		}


		public static Atom.Core.Collections.AtomEntryCollection geadGMailEmail(string username, string password) {
			// source found at https://app.box.com/shared/ka7j7n5bzm
			Atom.Core.Collections.AtomEntryCollection entries = null;

			WebRequest webRequestGetUrl;
			try {
				// Create a new web-request instance
				webRequestGetUrl = WebRequest.Create(_gmailServerUri);

				byte[] bytes = Encoding.ASCII.GetBytes(username + ":" + password);

				// Add the headers for basic authentication.
				webRequestGetUrl.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));

				// Get the response feed
				Stream feedStream = webRequestGetUrl.GetResponse().GetResponseStream();

				// Load Feed into Atom DLL
				Atom.Core.AtomFeed gmailFeed = Atom.Core.AtomFeed.Load(feedStream);

				//Atom.Core.Collections.AtomEntryCollection entries = gmailFeed.Entries;
				entries = gmailFeed.Entries;



				//if (entries.Count > 0) {
				//Console.WriteLine(string.Format("Found {0} email(s)", entries.Count));
				//Console.WriteLine(Environment.NewLine);

				// Read the first email in the inbox only.
				// you can change this section to read all emails.
				//for (int i = 0; i < 1; i++) {
				//     AtomEntry email = entries[i];
				//     Console.WriteLine("Brief details of the first Email in the inbox:");
				//     Console.WriteLine("Title: " + email.Title.Content);
				//     Console.WriteLine("Author Name: " + email.Author.Name);
				//     Console.WriteLine("Author Email: " + email.Author.Email);
				//     Console.WriteLine(Environment.NewLine);
				//     Console.WriteLine("Email Content: " + Environment.NewLine);
				//     Console.WriteLine(email.Summary.Content);
				//     Console.WriteLine(Environment.NewLine);
				//     Console.WriteLine("Hit 'Enter' to exit");
				//     Console.ReadLine();
				//}
				//}				
			}
			catch (Exception ex) {
			}

			return entries;
		}

		//public static List<EAGetMail.Mail> getGMailEmail_1(string inboxPath, string username, string password) {
		//	// Create a folder named "inbox" under current directory
		//	// to save the email retrieved.
		//	//string curpath = Directory.GetCurrentDirectory();
		//	List<EAGetMail.Mail> emails = new List<Mail>();

		//	//string mailbox = String.Format("{0}\\inbox", inboxPath);
		//	string mailbox = inboxPath;

		//	// If the folder is not existed, create it.
		//	//if (!Directory.Exists(mailbox)) {
		//	//     Directory.CreateDirectory(mailbox);
		//	//}

		//	// Gmail IMAP4 server is "imap.gmail.com"
		//	MailServer oServer = new MailServer("imap.gmail.com", username, password, ServerProtocol.Imap4);
		//	MailClient oClient = new MailClient("TryIt");

		//	// Set SSL connection,
		//	oServer.SSLConnection = true;




		//	// Set 993 IMAP4 port
		//	oServer.Port = 993;

		//	try {
		//		oClient.Connect(oServer);

		//		//Imap4Folder[] folders = oClient.Imap4Folders;
		//		Imap4Folder exportLabel = new Imap4Folder("export");
		//		oClient.SelectFolder(exportLabel);

		//		//EnumerateFolder(folders, oClient);

		//		MailInfo[] infos = oClient.GetMailInfos();

		//		for (int i = 0; i < infos.Length; i++) {
		//			MailInfo info = infos[i];


		//			// Receive email from GMail IMAP4 server
		//			Mail oMail = oClient.GetMail(info);

		//			emails.Add(oMail);
		//		}

		//		// Quit and pure emails marked as deleted from Gmail IMAP4 server.
		//		oClient.Quit();
		//	}
		//	catch (Exception ep) {
		//		Console.WriteLine(ep.Message);
		//	}
		//	return emails;
		//}

		public static List<Data.Email> getGMailEmail(string hostName, int port, bool useSSL, string username, string password) {
			List<Data.Email> emails = new List<Data.Email>();
			Data.Email email = null;

			bool isHtml = true;

			ImapX.ImapClient client = new ImapX.ImapClient(hostName, port, useSSL);
			bool result = false;

			result = client.Connection();


			result = client.LogIn(username, password);
			if (result) {
				ImapX.FolderCollection folders = client.Folders;

				ImapX.MessageCollection messages = client.Folders["export"].Search("ALL", true);	// .Search("export", true); //true - means all message parts will be received from server

				if (messages != null && messages.Count > 0) {
					for (int i = 0; i < messages.Count; i++) {
						email = new Data.Email();
						email.From = messages[i].From.DisplayName;
						email.Subject = messages[i].Subject;
						email.TextBody = messages[i].GetDecodedBody(out isHtml);

						//email.TextBody = messages[i].TextBody.TextData;
						email.ReceivedDate = messages[i].Date;


						emails.Add(email);
					}
				}
			}

			return emails;
		}

		public static List<Data.Email> getGMailEmail(string inboxPath, string username, string password) {
			List<Data.Email> emails = new List<Data.Email>();
			Data.Email email = null;

			bool isHtml = true;

			ImapX.ImapClient client = new ImapX.ImapClient("imap.gmail.com", 993, true);
			bool result = false;

			result = client.Connection();


			result = client.LogIn(username, password);
			if (result) {
				ImapX.FolderCollection folders = client.Folders;
				
				ImapX.MessageCollection messages = client.Folders["export"].Search("ALL", true);	// .Search("export", true); //true - means all message parts will be received from server

				if (messages != null && messages.Count > 0) {
					for (int i = 0; i <  messages.Count; i++) {
						email = new Data.Email();
						email.From = messages[i].From.DisplayName;
						email.Subject = messages[i].Subject;
						email.TextBody = messages[i].GetDecodedBody(out isHtml);

						//email.TextBody = messages[i].TextBody.TextData;
						email.ReceivedDate = messages[i].Date;

						
						emails.Add(email);
					}
				}
			}

			return emails;
		}

		
	}
}