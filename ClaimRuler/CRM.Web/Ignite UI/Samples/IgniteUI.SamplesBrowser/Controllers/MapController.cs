using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MapController : Controller
    {
        //
        // GET: /FunnelChart/
        [ActionName("binding-to-collection")]
        public ActionResult BindCollection()
        {
            //  Get the content of the world-cities.js file and extract the array data only
            //  excluding the opening variable declaration
            var jss = new JavaScriptSerializer();
            var jsFileName = Server.MapPath("~/data-files/world-cities.js");
            var jsFileContent = System.IO.File.ReadAllText(jsFileName);
            var openingBracePos = jsFileContent.IndexOf('[');
            var closingBracePos = jsFileContent.IndexOf(']');
            var jsonContent = jsFileContent.Substring(openingBracePos, closingBracePos - openingBracePos + 1);

            //  De-serialize WorldCities data from a JavaScript array into List<WorldCity>
            var wordlCities = jss.Deserialize<List<WorldCity>>(jsonContent);

            return View("binding-to-collection", wordlCities.AsQueryable());
        }
    }

    public class WorldCity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
