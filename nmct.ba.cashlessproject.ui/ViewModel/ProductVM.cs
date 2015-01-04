using GalaSoft.MvvmLight;
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
using System.Windows;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class ProductVM : ObservableObject, IPage
    {
        //properties
        private bool _isAddingProduct;
        public bool IsAddingProduct
        {
            get { return _isAddingProduct; }
            set { _isAddingProduct = value; OnPropertyChanged("IsAddingProduct"); OnPropertyChanged("OverviewVisibility"); OnPropertyChanged("AddingProductVisibility"); }
        }

        public string Name
        {
            get { return "Producten"; }
        }

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
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        //constructor
        public ProductVM()
        {
            if (Products == null)
            {
                GetProducts();
            }
        }

        //visibilities
        public Visibility OverviewVisibility
        {
            get { if (!IsAddingProduct) return Visibility.Visible; return Visibility.Collapsed; }
        }

        public Visibility AddingProductVisibility
        {
            get { if (!IsAddingProduct) return Visibility.Collapsed; return Visibility.Visible; }
        }

        //commands
        public ICommand ChangeProductPage
        {
            get { return new RelayCommand(CheckCommand); }
        }

        public ICommand AddProduct
        {
            get { return new RelayCommand(CheckProduct); }
        }

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }

        public ICommand EditProduct
        {
            get { return new RelayCommand(EditProd); }
        }

        //methods
        public void CheckCommand()
        {
            IsAddingProduct = true;

            Product p = new Product();
            Products.Add(p);
            SelectedProduct = p;
        }

        public void CheckProduct()
        {  
            if(SelectedProduct.Id == 0)
            {
                if (SelectedProduct.Name != null && SelectedProduct.Price != null)
                {
                    AddNewProduct(SelectedProduct);
                    IsAddingProduct = false;
                }
                else
                {
                    Error = "Alle velden zijn verplicht!";
                }
            }
            else
            {
                if (SelectedProduct != null && SelectedProduct.Name != null && SelectedProduct.Price != null)
                {
                    UpdateProduct(SelectedProduct);
                    IsAddingProduct = false;
                }
                else
                {
                    Error = "Alle velden zijn verplicht!";
                }
            }
        }

        public void EditProd()
        {
            if(SelectedProduct != null)
            {
                IsAddingProduct = true;
            }
        }

        //json
        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                }
            }
        }

        public async void AddNewProduct(Product NewProd)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string product = JsonConvert.SerializeObject(NewProd);

                HttpResponseMessage response = await client.PostAsync("http://localhost:1428/api/product", new StringContent(product, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Add Product");
                }
            }
        }

        public async void DeleteProduct()
        {
            if(SelectedProduct != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync("http://localhost:1428/api/product/" + SelectedProduct.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        Products.Remove(SelectedProduct);
                    }
                    else
                    {
                        Console.WriteLine("Delete Error");
                    }
                }
            }
        }

        public async void UpdateProduct(Product UpdatedProd)
        {
            using(HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string product = JsonConvert.SerializeObject(UpdatedProd);

                HttpResponseMessage response = await client.PutAsync("http://localhost:1428/api/product", new StringContent(product, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Update Product");
                }
            }
        }
    }
}
