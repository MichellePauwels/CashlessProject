using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model.Model.Costumer
{
    class ErrorlogCostumer
    {
        private int _registerId;
        public int RegisterId
        {
            get { return _registerId; }
            set { _registerId = value; }
        }

        private DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private int _stackTrace;
        public int StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }
    }
}
