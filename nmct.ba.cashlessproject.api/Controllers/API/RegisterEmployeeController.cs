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
    public class RegisterEmployeeController : ApiController
    {
        public List<Employee> Get(int id)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetEmployeePerRegister(id, cp.Claims);
        }

        public HttpResponseMessage Post(RegisterEmployee regemp)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = RegisterEmployeeDA.LinkEmployeeToRegister(regemp, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }
    }
}
