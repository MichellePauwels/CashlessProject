using nmct.ba.cashlessproject.api.Models.DA;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers.API
{
    public class VerenigingController : ApiController
    {
        public HttpResponseMessage Post(Vereniging ver)
        {
            VerenigingDA.InsertVereniging(ver);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(Vereniging ver)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            VerenigingDA.UpdatePassword(cp.Claims, ver);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
