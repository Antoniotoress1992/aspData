using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Converters;

namespace IgniteUI.SamplesBrowser.Application.Model
{

    /// <summary>
    /// Handles JsonP requests when requests are fired with 
    /// text/javascript or application/json and contain 
    /// a callback= (configurable) query string parameter
    /// 
    /// Based on Christian Weyers implementation
    /// https://github.com/thinktecture/Thinktecture.Web.Http/blob/master/Thinktecture.Web.Http/Formatters/JsonpFormatter.cs
    /// Adapted from Rick Strahl's blog: http://www.west-wind.com/weblog/posts/2012/Apr/02/Creating-a-JSONP-Formatter-for-ASPNET-Web-API
    /// </summary>
    public class JsonpFormatter : JsonMediaTypeFormatter
    {

        public JsonpFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            JsonpParameterName = "callback";
        }

        /// <summary>
        ///  Name of the query string parameter to look for
        ///  the jsonp function name
        /// </summary>
        public string JsonpParameterName { get; set; }

        /// <summary>
        /// Captured name of the Jsonp function that the JSON call
        /// is wrapped in. Set in GetPerRequestFormatter Instance
        /// </summary>
        private string JsonpCallbackFunction;


        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>
        /// Override this method to capture the Request object
        /// and look for the query string parameter and 
        /// create a new instance of this formatter.
        /// 
        /// This is the only place in a formatter where the
        /// Request object is available.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="request"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            var formatter = new JsonpFormatter()
            {
                JsonpCallbackFunction = GetJsonCallbackFunction(request)
            };

            formatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            formatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return formatter;
        }

        /// <summary>
        /// Override to wrap existing JSON result with the
        /// JSONP function call
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="content"></param>
        /// <param name="transportContext"></param>
        /// <returns></returns>
        /// 
        public override Task WriteToStreamAsync(Type type, object value,
            Stream stream, HttpContent content, TransportContext transportContext)
        {
            if (string.IsNullOrEmpty(JsonpCallbackFunction))
                return base.WriteToStreamAsync(type, value, stream, content, transportContext);

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(stream);
                writer.Write(JsonpCallbackFunction + "({ \"d\" : { \"results\": ");
                //writer.Write(JsonpCallbackFunction + "(");
                writer.Flush();
            }
            catch (Exception ex)
            {
                try
                {
                    if (writer != null)
                        writer.Dispose();
                }
                catch { }

                var tcs = new TaskCompletionSource<object>();
                tcs.SetException(ex);
                return tcs.Task;
            }

            return base.WriteToStreamAsync(type, value, stream, content,
                    transportContext).ContinueWith(innerTask =>
                        {
                            if (innerTask.Status == TaskStatus.RanToCompletion)
                            {
                                //writer.Write(")");
                                writer.Write("}})");
                                writer.Flush();
                            }

                        }, TaskContinuationOptions.ExecuteSynchronously)
                        .ContinueWith(innerTask =>
                            {
                                writer.Dispose();
                                return innerTask;

                            }, TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap();
        }

        /// <summary>
        /// Retrieves the Jsonp Callback function
        /// from the query string
        /// </summary>
        /// <returns></returns>
        private string GetJsonCallbackFunction(HttpRequestMessage request)
        {
            if (request.Method != HttpMethod.Get || request.RequestUri == null)
                return null;

            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var queryVal = query[this.JsonpParameterName];

            if (string.IsNullOrEmpty(queryVal))
                return null;

            return queryVal;
        }
    }
}
