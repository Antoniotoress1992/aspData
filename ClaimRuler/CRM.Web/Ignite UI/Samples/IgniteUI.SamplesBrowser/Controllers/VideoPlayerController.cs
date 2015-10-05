using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class VideoPlayerController : Controller
    {

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspNetMvcHelper()
        {
            ViewData["videoSources"] = new List<string>() {
                "http://dl.infragistics.com/pg/2011-1/web/shared/videoplayer/videos/Infragistics_Presentation_lowRes_1.h264.mp4",
                "http://dl.infragistics.com/pg/2011-1/web/shared/videoplayer/videos/Infragistics_Presentation_lowRes_1.webmvp8.webm",
                "http://dl.infragistics.com/pg/2011-1/web/shared/videoplayer/videos/Infragistics_Presentation_lowRes_1.theora.ogv" };
            ViewData["posterUrl"] = Url.Content("~/images/samples/video-player/ig-pres.png");


            return View("aspnet-mvc-helper");
        }

        [ActionName("fallback-video")]
        public ActionResult FallbackVideo()
        {
            ViewData["videoSources"] = new List<string>() {
                "http://dl.infragistics.com/pg/2011-1/web/shared/videoplayer/videos/QuinceIntro_1.h264.mp4",
                "http://dl.infragistics.com/pg/2011-1/web/shared/videoplayer/videos/QuinceIntro_1.webmvp8.webm",
                "http://dl.infragistics.com/pg/2011-1/web/shared/videoplayer/videos/QuinceIntro_1.theora.ogv" };
            ViewData["posterUrl"] = Url.Content("~/images/samples/video-player/quince-intro-1.png");

            return View("fallback-video");
        }
    }
}
