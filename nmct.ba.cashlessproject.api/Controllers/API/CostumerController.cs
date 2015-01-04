using nmct.ba.cashlessproject.api.Models.DA;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class CostumerController : ApiController
    {
        public List<Costumer> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return CostumerDA.GetCostumers(cp.Claims);
        }

        public Costumer Get(int id)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return CostumerDA.GetCostumerById(id, cp.Claims);
        }

        public HttpResponseMessage Post(Costumer cost)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = CostumerDA.InsertCostumer(cost, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Costumer cost)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            CostumerDA.UpdateCostumer(cost, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}