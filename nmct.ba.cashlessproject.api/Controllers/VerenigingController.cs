using nmct.ba.cashlessproject.api.Models.DA;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]
    public class VerenigingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Overzicht()
        {
            List<Vereniging> listorganisations = VerenigingDA.GetOrganisations();
            ViewBag.ListOrganisations = listorganisations;
            return View();
        }

        public ActionResult Registreren()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertVereniging(string vernaam, string adres, string email, string telefoon, string loginUser, string loginPas, string dbName, string dbUser, string dbPas)
        {
            if (!string.IsNullOrEmpty(vernaam) && !string.IsNullOrEmpty(adres) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(telefoon) && !string.IsNullOrEmpty(loginUser) && !string.IsNullOrEmpty(loginPas) && !string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPas))
            {
                Vereniging ver = new Vereniging()
                {
                    OrganisationName = vernaam,
                    Address = adres,
                    Email = email,
                    Phone = telefoon,
                    Login = loginUser,
                    Password = loginPas,
                    DbName = dbName,
                    DbLogin = dbUser,
                    DbPassword = dbPas
                };

                VerenigingDA.InsertVereniging(ver);

                return RedirectToAction("Overzicht");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(int orgId)
        {
            if(orgId != null && orgId != 0)
            {
                Vereniging ver = VerenigingDA.GetOrganisationById(orgId);

                return View(ver);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult EditVereniging(int orgId)
        {
            if (orgId != null && orgId != 0)
            {
                Vereniging ver = VerenigingDA.GetOrganisationById(orgId);

                return View(ver);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UpdateVereniging(Vereniging ver)
        {
            if(ver != null)
            {
                VerenigingDA.UpdateVereniging(ver);
                return RedirectToAction("Overzicht");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}