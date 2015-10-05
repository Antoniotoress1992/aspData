using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IgniteUI.SamplesBrowser.Application.Model.Util
{
    public class SvgHttpHandler:IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string relFilePath = context.Request.Url.LocalPath;
            string svgFile = "";

            if (!string.IsNullOrEmpty(relFilePath))
            {
                svgFile = context.Server.MapPath(relFilePath);
            }

            if (!string.IsNullOrEmpty(svgFile) && File.Exists(svgFile))
            {
                context.Response.ContentType = "image/svg+xml";
                context.Response.WriteFile(svgFile);
            }
            else
            {
                throw new HttpException(404, "SVG resource not found");
            }
        }
    }
}