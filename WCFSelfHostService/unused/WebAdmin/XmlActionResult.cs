using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;


namespace VTCTIManagerService.WebAdmin
{
    public sealed class XmlActionResult : IHttpActionResult
    {
        private readonly XDocument _document;
        public string MimeType { get; set; }
        private HttpStatusCode code;

        public XmlActionResult(XDocument document, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            _document = document;

            this.code = code;

            // Default values
            MimeType = "application/xml";
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = new HttpResponseMessage(code);
                response.Content = new StringContent(_document.ToString());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);

                response.Content.Headers.Expires = DateTime.Now;

                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoCache = true;
                response.Headers.CacheControl.NoStore = true;

                response.Content.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content.Headers.Add("Access-Control-Max-Age", "3600");
                response.Content.Headers.Add("Access-Control-Allow-Headers", "x-requested-with");

                string[] methods = { "POST", "GET", "PUT", "OPTIONS", "DELETE"};                
                response.Content.Headers.Add("Access-Control-Allow-Methods", methods);

                return Task.FromResult(response);
            }
            catch(Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Internal Server Error. " + e.Message);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

                response.Content.Headers.Expires = DateTime.Now;

                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoCache = true;
                response.Headers.CacheControl.NoStore = true;

                response.Content.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content.Headers.Add("Access-Control-Max-Age", "3600");
                response.Content.Headers.Add("Access-Control-Allow-Headers", "x-requested-with");

                string[] methods = { "POST", "GET", "PUT", "OPTIONS", "DELETE" };
                response.Content.Headers.Add("Access-Control-Allow-Methods", methods);

                return Task.FromResult(response);
            }
        }
    }
}
