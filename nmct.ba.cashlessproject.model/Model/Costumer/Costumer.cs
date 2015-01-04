using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model.Model.Costumer
{
    public class Costumer
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _costumerName;
        public string CostumerName
        {
            get { return _costumerName; }
            set { _costumerName = value; }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private byte[] _imagePath;
        public byte[] ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        private string _rijksregisternummer;
        public string Rijksregisternummer
        {
            get { return _rijksregisternummer; }
            set { _rijksregisternummer = value; }
        }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
    }
}
