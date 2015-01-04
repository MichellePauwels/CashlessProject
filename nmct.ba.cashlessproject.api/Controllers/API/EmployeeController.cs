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
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployees(cp.Claims);
        }

        public HttpResponseMessage Post(Employee emp)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = EmployeeDA.InsertEmployee(emp, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Employee emp)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.UpdateEmployee(emp, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.DeleteEmployee(id, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}