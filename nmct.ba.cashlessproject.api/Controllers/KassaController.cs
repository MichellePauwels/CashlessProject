using nmct.ba.cashlessproject.api.Models.DA;
using nmct.ba.cashlessproject.api.Models.Presentation;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]    
    public class KassaController : Controller
    {
        List<Register> allregisters = RegisterDA.GetRegisters();
        List<Vereniging> allorganisations = VerenigingDA.GetOrganisations();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Link()
        {
            PMKassa pmkassa = new PMKassa();
            pmkassa.ListOrganisations = allorganisations;
            pmkassa.ListRegisters = allregisters;
            pmkassa.ListAvailableRegisters = new List<Register>();

            foreach (Register reg in allregisters)
            {
                bool isAvailableRegister = RegisterOrganisationDA.IsAvailableRegister(reg.Id);

                if (isAvailableRegister)
                {
                    pmkassa.ListAvailableRegisters.Add(reg);
                }
            }

            return View(pmkassa);
        }

        [HttpPost]
        public ActionResult LinkKassa(string kassa, string vereniging)
        {
            if(!string.IsNullOrEmpty(kassa) && !string.IsNullOrEmpty(vereniging))
            {
                Vereniging selectedVereniging = VerenigingDA.GetOrganisationById(Convert.ToInt32(vereniging));
                Register selectedRegister = RegisterDA.GetRegisterById(Convert.ToInt32(kassa));

                RegisterOrganisationDA.LinkRegisterToOrganisation(selectedRegister, selectedVereniging);

                return RedirectToAction("Link");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult Overzicht()
        {
            PMKassa pmkassa = new PMKassa();
            pmkassa.ListOrganisations = allorganisations;
            pmkassa.ListRegisters = allregisters;
            pmkassa.ListAvailableRegisters = new List<Register>();

            foreach(Register reg in allregisters)
            {
                bool isAvailableRegister = RegisterOrganisationDA.IsAvailableRegister(reg.Id);

                if(isAvailableRegister)
                {
                    pmkassa.ListAvailableRegisters.Add(reg);
                }
            }

            return View(pmkassa);
        }

        [HttpPost]
        public ActionResult Overzicht(string vereniging)
        {
            PMKassa pmkassa = new PMKassa();
            pmkassa.ListRegisters = RegisterDA.GetRegisterPerOrganisation(Convert.ToInt32(vereniging));
            pmkassa.ListOrganisations = allorganisations;
            pmkassa.ListAvailableRegisters = new List<Register>();

            foreach (Register reg in allregisters)
            {
                bool isAvailableRegister = RegisterOrganisationDA.IsAvailableRegister(reg.Id);

                if (isAvailableRegister)
                {
                    pmkassa.ListAvailableRegisters.Add(reg);
                }
            }

            return View(pmkassa);
        }

        [HttpGet]
        public ActionResult Toevoegen()
        {
            Register register = new Register();
            return View(register);
        }

        [HttpGet]
        public ActionResult Wijzig(string RegisterId)
        {
            ViewBag.ChangeRegister = RegisterDA.GetRegisterById(Convert.ToInt32(RegisterId));
            ViewBag.ListOrganisations = allorganisations;

            return View();
        }

        [HttpPost]
        public ActionResult WijzigKassa(string changeregisterid, string vereniging)
        {
            if(!string.IsNullOrEmpty(changeregisterid) && !string.IsNullOrEmpty(vereniging))
            {
                Vereniging selectedVereniging = VerenigingDA.GetOrganisationById(Convert.ToInt32(vereniging));
                Register selectedRegister = RegisterDA.GetRegisterById(Convert.ToInt32(changeregisterid));

                //RegisterOrganisationDA.LinkRegisterToOrganisation(selectedRegister, selectedVereniging);

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

        [HttpPost]
        public ActionResult ToevoegenKassa(Register register)
        {
            if(register != null)
            {
                RegisterDA.InsertRegister(register);

                return RedirectToAction("Overzicht");
            }
            else
            {
                return RedirectToAction("Error");
            } 
        }
    }
}