using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using Grapevine.Server;
using QuantConnect;
using QuantConnect.Lean.Engine.Server;

namespace LeanManager
{
    public sealed class GrapevineGetResource : RESTResource
    {



        [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/JSON/portfolio")]
        public void HandleCallsRequests(HttpListenerContext context)
        {
            if (RaysLeanManager.Algorithm == null)
            {
                this.SendJsonResponse(context, new grapevine_error(-1, "still loading"));
                return;
            }


            this.SendJsonResponse(context, new Datatables(RaysLeanManager.Algorithm.Portfolio.Values.ToArray()));

        }


        [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/JSON/scan")]
        public void HandlePhoneRequests(HttpListenerContext context)
        {
            if (RaysLeanManager.Algorithm == null)
            {
                this.SendJsonResponse(context, new grapevine_error(-1, "still loading"));
                return;
            }
            RaysLeanManager.Algorithm.Scan();

            this.SendJsonResponse(context, new grapevine_error(0, "ok"));


        }


        [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/JSON/manager")]
        public void HandleManagerRequests(HttpListenerContext context)
        {
            if (RaysLeanManager.Algorithm == null)
            {
                this.SendJsonResponse(context, new grapevine_error(-1, "still loading"));
                return;
            }


            this.SendJsonResponse(context, new Datatables(RaysLeanManager.Algorithm.Manager.Values.ToArray()));

        }

        [RESTRoute(Method = HttpMethod.GET, PathInfo = @"^/JSON/history")]
        public void HandlePriceHistoryRequests(HttpListenerContext context)
        {
            if (RaysLeanManager.Algorithm == null)
            {
                this.SendJsonResponse(context, new grapevine_error(-1, "still loading"));
                return;
            }


            this.SendJsonResponse(context, new Datatables(RaysLeanManager.Algorithm.History(Symbol.Create("AAPL",SecurityType.Equity,"usa"),TimeSpan.FromDays(5),Resolution.Minute)));

        }

        [RESTRoute]
        public void HandleAllGetRequests(HttpListenerContext context)
        {
            String URL = "";
            if (context.Request.RawUrl.Contains("?"))
            {
                URL = context.Request.RawUrl.Substring(0, context.Request.RawUrl.IndexOf("?"));
            }
            else
            {
                URL = context.Request.RawUrl;
            }


            String FileName = URL;
            if (URL.Equals("/"))
            {
                FileName = "index.html";
            }

            FileName = (Environment.CurrentDirectory + "\\Web\\" + FileName).Replace("/", "\\");

            if (File.Exists(FileName))
            {
                HttpListenerResponse response = context.Response;
                byte[] buffer;
                if ((FileName.EndsWith(".html") || FileName.EndsWith(".htm") || FileName.EndsWith(".txt") || FileName.EndsWith(".json")))
                {
                    var responseString = File.ReadAllText(FileName);
                    buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                }
                else
                {
                    buffer = File.ReadAllBytes(FileName);
                }
                //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            else
            {
                HttpListenerResponse response = context.Response;
                var responseString = "404 file not found: " + FileName;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.StatusCode = 404;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
    }

    public class Datatables
    {
        public Object data;

        public Datatables(object Data)
        {
            data = Data;
        }
    }
}
