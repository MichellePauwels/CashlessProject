using nmct.ba.cashlessproject.api.Models.DA;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models.Presentation
{
    public class PMKassa
    {
        public List<Register> ListRegisters { get; set; }
        public List<Register> ListAvailableRegisters { get; set; }
        public List<Vereniging> ListOrganisations { get; set; }
    }
}