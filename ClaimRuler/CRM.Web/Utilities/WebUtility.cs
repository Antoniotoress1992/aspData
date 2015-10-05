
namespace CRM.Web.Utilities {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	public class WebUtility {
		public static bool IsAuthenticated() {
			return HttpContext.Current.User.Identity.IsAuthenticated;
		}

		public static int GetCurrentUserId() {
			return Convert.ToInt32(HttpContext.Current.User.Identity.Name);
		}



	}

	//    public static class cln
	//{
	//	   public static T Clone<T>(this T source)
	//	   {
	//		  var dcs = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
	//		  using (var ms = new System.IO.MemoryStream())
	//		  {
	//			 dcs.WriteObject(ms, source);
	//			 ms.Seek(0, System.IO.SeekOrigin.Begin);
	//			 return (T)dcs.ReadObject(ms);
	//		  }
	//	   }
	//}
}