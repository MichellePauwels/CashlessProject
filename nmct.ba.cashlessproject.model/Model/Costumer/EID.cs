using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model.Model.Costumer
{
    public class EID
    {
        private string _firstname;
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        private string _rijksregisternummer;
        public string Rijksregisternummer
        {
            get { return _rijksregisternummer; }
            set { _rijksregisternummer = value; }
        }

        private string _street;
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        private byte[] _photo;
        public byte[] Photo
        {
            get { return _photo; }
            set { _photo = value; }
        }
        
    }
}
