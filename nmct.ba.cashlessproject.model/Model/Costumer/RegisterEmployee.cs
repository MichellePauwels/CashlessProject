using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model.Model.Costumer
{
    public class RegisterEmployee
    {
        private int _registerId;
        public int RegisterId
        {
            get { return _registerId; }
            set { _registerId = value; }
        }

        private int _employeeId;
        public int EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        private DateTime _from;
        public DateTime From
        {
            get { return _from; }
            set { _from = value; }
        }

        private DateTime _until;
        public DateTime Until
        {
            get { return _until; }
            set { _until = value; }
        }
        
        
    }
}
