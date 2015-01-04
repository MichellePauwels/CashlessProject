using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class EditorProductVM : ObservableObject, IPage
    {
        private Product _newProduct = new Product();
        public Product NewProduct
        {
            get { return _newProduct; }
            set { _newProduct = value; OnPropertyChanged("NewProduct"); }
        }

        public EditorProductVM(string name, double price)
        {
            NewProduct.Name = name;
            NewProduct.Price = price;
        }

        public EditorProductVM()
        {

        }
        
        public string Name
        {
            get { return "Toevoegen"; }
        }
    }
}
