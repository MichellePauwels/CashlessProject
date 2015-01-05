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

        [HttpGet]
        public ActionResult Registreren()
        {
            Vereniging vereniging = new Vereniging();
            return View(vereniging);
        }

        [HttpPost]
        public ActionResult InsertVereniging(Vereniging newOrganisation)
        {
            if (newOrganisation != null)
            {
                VerenigingDA.InsertVereniging(newOrganisation);

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