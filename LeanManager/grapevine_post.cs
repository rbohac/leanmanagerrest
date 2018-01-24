using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using Grapevine.Server;
using Newtonsoft.Json;
using QuantConnect;
using QuantConnect.Lean.Engine.Server;

namespace LeanManager
{
    public sealed class GrapevinePutResource : RESTResource
    {
   
        [RESTRoute(Method = HttpMethod.POST, PathInfo = @"^/scan")]
        public void HandlePhoneRequests(HttpListenerContext context)
        {
            if (RaysLeanManager.Algorithm == null)
            {
                this.SendJsonResponse(context, new grapevine_error(-1, "still loading"));
                return;
            }
            RaysLeanManager.Algorithm.Scan();

            this.SendJsonResponse(context, new grapevine_error(0,"ok"));


        }

        [RESTRoute(Method = HttpMethod.POST, PathInfo = @"^/JSON/LIQUIDATEALL")]
        public void HandleLiquidateAllRequests(HttpListenerContext context)
        {
            if (RaysLeanManager.Algorithm == null)
            {
                this.SendJsonResponse(context, new grapevine_error(-1, "still loading"));
                return;
            }
            RaysLeanManager.Algorithm.Liquidate();

            this.SendJsonResponse(context, new grapevine_error(0, "ok"));


        }
    }
}
