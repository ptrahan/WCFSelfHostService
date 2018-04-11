using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;


namespace VTCTIManagerService.WebAdmin
{
    public class HtmlActionResult : IHttpActionResult
    {
        private const string ViewDirectory = @"Views\";
        private readonly string _view;
        private readonly dynamic _model;

        public HtmlActionResult(string viewName, dynamic model)
        {
            _view = null;

            if (viewName != null)
                _view = LoadView(viewName);

            _model = model;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            if(_view == null)
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.Content = new StringContent("404 Not found. The resource was not found on the server.");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return Task.FromResult(response);
            }

            response = new HttpResponseMessage(HttpStatusCode.OK);
            var parsedView = RazorEngine.Razor.Parse(_view, _model);
            response.Content = new StringContent(parsedView);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return Task.FromResult(response);
        }

        private static string LoadView(string name)
        {
            var view = File.ReadAllText(Path.Combine(ViewDirectory, name + ".cshtml"));
            return view;
        }
    }
}
