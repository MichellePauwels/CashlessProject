using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class OverviewProductVM : ObservableObject, IPage
    {
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); } //onproperty kan pas als je : ObservableObject
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedShop"); }
        }

        public OverviewProductVM()
        {
            if(Products == null)
            {
                GetProducts();
            }
        }

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                }
            }
        }

        public async void DeleteProduct()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:1428/api/product/" + SelectedProduct.Id);
                if (response.IsSuccessStatusCode)
                {
                    Products.Remove(SelectedProduct);
                }
            }
        }

        private void ToevoegenPagina(Product prod)
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new EditorProductVM(prod.Name, prod.Price));
        }

        //commands
        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }

        public ICommand ToevoegenCommand
        {
            get { return new RelayCommand<Product>(ToevoegenPagina); }
        }

        public string Name
        {
            get { return "Overzicht"; }
        }
    }
}
