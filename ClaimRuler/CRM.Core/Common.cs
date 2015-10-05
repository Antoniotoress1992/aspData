namespace CRM.Core {
	#region Namespaces
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.IO;
	using System.Linq;
	using System.Web;
	using System.Web.Security;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Data;
	using CRM.Data.Account;
    using System.Net;
	#endregion

	public class Common {
		public static string[] ReminderDialogStrings = { "Week", "Weeks", "Day", "Days", "Hour", "Hours", "Minute", "Minutes", "Now" };

		public static string getAdjusterPhotoURL(int adjusterId, string photoFileName) {
			string photoPath = null;

			photoPath = string.Format("{0}/Adjusters/{1}/Photo/{2}", ConfigurationManager.AppSettings["appPath"], adjusterId, photoFileName);

			if (string.IsNullOrEmpty(photoFileName) || !File.Exists(photoPath))
				photoPath = "~/images/user-thumbnail.png";
			else
				photoPath = string.Format("{0}/Adjusters/{1}/Photo/{2}", ConfigurationManager.AppSettings["appURL"], adjusterId, photoFileName);
				

			return photoPath;
		}

		public static IDictionary<string, string> GetDays() {
			return new Dictionary<string, string>()
                     {
                        {"Sunday", "Sunday"},
                        {"Monday", "Monday"},
                        {"Tuesday", "Tuesday"},
                        {"Wednesday", "Wednesday"},
                        {"Thursday", "Thursday"},
                        {"Friday", "Friday"},
                        {"Saturday", "Saturday"}
                     };
		}

		public static string getUserPhotoURL(int userID) {
			string photoPath = null;
			
			photoPath = string.Format("{0}/UserPhoto/{1}.jpg", ConfigurationManager.AppSettings["appPath"], userID);

			if (System.IO.File.Exists(photoPath))
					photoPath = string.Format("{0}/UserPhoto/{1}.jpg", ConfigurationManager.AppSettings["appURL"], userID);
				else
					photoPath = "~/images/user-thumbnail.png";

			return photoPath;
		}


		public static IDictionary<int, int> GetYearsRange(int min, int max) {
			return Enumerable.Range(min, max).ToDictionary(k => k, v => v);
		}

		public static IDictionary<string, string> GetMonths() {
			return Enumerable.Range(1, 12).ToDictionary(k => k.ToString(), v => v.ToString());
		}

		public static IDictionary<string, string> GetYears() {
			return Enumerable.Range(DateTime.Now.Year, 15).ToDictionary(k => k.ToString(), v => v.ToString());
		}

		public static string convertIntervalToString(int interval) {
			string returnString = null;
			
			var minutes = interval;
			var hours = minutes / 60;
			var days = hours / 24;
			var weeks = days / 7;

			if (weeks == 1)
				returnString = "1 " + ReminderDialogStrings[0];
			else if (weeks > 1)
				returnString = weeks + " " + ReminderDialogStrings[1];
			else if (days == 1)
				returnString = "1 " + ReminderDialogStrings[2];
			else if (days > 1)
				returnString = days + " " + ReminderDialogStrings[3];
			else if (hours == 1)
				returnString = "1 " + ReminderDialogStrings[4];
			else if (hours > 1)
				returnString = hours + " " + ReminderDialogStrings[5];
			else if (minutes == 1)
				returnString = "1 " + ReminderDialogStrings[6];
			else if (minutes > 1 || minutes == 0)
				returnString = minutes + " " + ReminderDialogStrings[7];
			else
				returnString = ReminderDialogStrings[8];

			return returnString;
		}
		public static string calculateOverdueTime(TimeSpan time) {
			string returnString = "";

			var minutes = Math.Abs(time.Minutes);
			var hours = Math.Abs(time.Hours);
			var days = Math.Abs(time.Days);
			var weeks = days / 7;

			if (weeks == 1)
				returnString = "1 " + ReminderDialogStrings[0];
			else if (weeks > 1)
				returnString = " " + weeks + " " + ReminderDialogStrings[1];

			if (days == 1)
				returnString += " 1 " + ReminderDialogStrings[2];
			else if (days > 1)
				returnString += " " + days + " " + ReminderDialogStrings[3];

			if (hours == 1)
				returnString += " 1 " + ReminderDialogStrings[4];
			else if (hours > 1)
				returnString += " " + hours + " " + ReminderDialogStrings[5];
			
			if (minutes == 1)
				returnString += " 1 " + ReminderDialogStrings[6];
			else if (minutes > 1 || minutes == 0)
				returnString += " " + minutes + " " + ReminderDialogStrings[7];
			

			return returnString + " Overdue";
		}

		public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size) {
			//Get the image current width
			int sourceWidth = imgToResize.Width;
			//Get the image current height
			int sourceHeight = imgToResize.Height;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;
			//Calulate  width with new desired size
			nPercentW = ((float)size.Width / (float)sourceWidth);
			//Calculate height with new desired size
			nPercentH = ((float)size.Height / (float)sourceHeight);

			if (nPercentH < nPercentW)
				nPercent = nPercentH;
			else
				nPercent = nPercentW;
			//New Width
			int destWidth = (int)(sourceWidth * nPercent);
			//New Height
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage((System.Drawing.Image)b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			// Draw image with new width and height
			g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose();
			return (System.Drawing.Image)b;
		}

        public static void SendRequest(string requestUri)
        {
            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader responseStreamReader = new StreamReader(responseStream);
                string responseData = responseStreamReader.ReadToEnd();

            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return;
        }



	}
}
