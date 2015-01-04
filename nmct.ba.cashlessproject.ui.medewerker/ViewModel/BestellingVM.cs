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

namespace nmct.ba.cashlessproject.ui.medewerker.ViewModel
{
    public class BestellingVM : ObservableObject, IPage
    {
        //properties
        private bool _isAnderePagina;
        public bool IsAnderePagina
        {
            get { return _isAnderePagina; }
            set { _isAnderePagina = value; OnPropertyChanged("IsAnderePagina"); OnPropertyChanged("BestellingVisibility"); OnPropertyChanged("AndereVisibility"); }
        }

        private bool _isOverTotaal;
        public bool IsOverTotaal
        {
            get { return _isOverTotaal; }
            set { _isOverTotaal = value; OnPropertyChanged("IsOverTotaal"); OnPropertyChanged("Totaal"); OnPropertyChanged("IsOverSaldoVisibility"); }
        }
        
        public string Name
        {
            get { return "Bestelling"; }
        }

        private double _totaal;
        public double Totaal
        {
            get { return _totaal; }
            set { _totaal = value; OnPropertyChanged("Totaal"); OnPropertyChanged("ProductsBestelling"); OnPropertyChanged("IsOverTotaal"); OnPropertyChanged("IsOverSaldoVisibility"); }
        }

        private int _selectedProductIndex;
        public int SelectedProductIndex
        {
            get { return _selectedProductIndex; }
            set { _selectedProductIndex = value; }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; }
        } 

        private ObservableCollection<Product> _productsBestelling;
        public ObservableCollection<Product> ProductsBestelling
        {
            get { return _productsBestelling; }
            set { _productsBestelling = value; OnPropertyChanged("ProductsBestelling"); }
        }

        private ObservableCollection<Product> _availableProducts;
        public ObservableCollection<Product> AvailableProducts
        {
            get { return _availableProducts; }
            set { _availableProducts = value; OnPropertyChanged("AvailableProducts"); }
        }
        private ObservableCollection<Product> _priorityProducts;
        public ObservableCollection<Product> PriorityProducts
        {
            get { return _priorityProducts; }
            set { _priorityProducts = value; OnPropertyChanged("PriorityProducts"); }
        }

        private ObservableCollection<Product> _otherProducts;
        public ObservableCollection<Product> OtherProducts
        {
            get { return _otherProducts; }
            set { _otherProducts = value; OnPropertyChanged("OtherProducts"); }
        }

        //constructor
        public BestellingVM()
        {
            if(AvailableProducts == null)
            {
                GetProducts();
            }
        }

        //visibilities
        public Visibility BestellingVisibility
        {
            get { if (!IsAnderePagina) return Visibility.Visible; return Visibility.Collapsed; }
        }

        public Visibility AndereVisibility
        {
            get { if (!IsAnderePagina) return Visibility.Collapsed ; return Visibility.Visible; }
        }

        public Visibility IsOverSaldoVisibility
        {
            get { if (IsOverTotaal) return Visibility.Visible; return Visibility.Hidden; }
        }

        //commands 
        public ICommand ChangeAnderePage
        {
            get { return new RelayCommand(CheckCommand); }
        }

        public ICommand AddProductCommand
        {
            get { return new RelayCommand<Product>(AddProductToList); }
        }

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProductFromList); }
        }

        public ICommand OpslaanBestellingCommando
        {
            get { return new RelayCommand(UpdateSaldo); }
        }

        //methods
        public void CheckCommand()
        {
            IsAnderePagina = true;
        }

        private void AddProductToList(Product product)
        {
            IsAnderePagina = false;
            IsOverTotaal = false;

            if(product != null)
            {
                if(ProductsBestelling == null)
                {
                    ProductsBestelling = new ObservableCollection<Product>();
                }

                ProductsBestelling.Add(product);

                Totaal += product.Price;
                if(Totaal > KaartVM.LoggedInCostumer.Balance)
                {
                    IsOverTotaal = true;
                }
            }
        }

        private void DeleteProductFromList()
        {
            if (SelectedProduct != null)
            {
                Totaal -= SelectedProduct.Price;

                ProductsBestelling.RemoveAt(SelectedProductIndex);
                if (Totaal > KaartVM.LoggedInCostumer.Balance)
                {
                    IsOverTotaal = true;
                }
                else
                {
                    IsOverTotaal = false;
                }
            }
        }

        private void OpslaanBestelling()
        {
            int costumerId = KaartVM.LoggedInCostumer.Id;
            DateTime timestamp = DateTime.Now;
            int registerId = 1;
            List<Product> listproducts = new List<Product>(ProductsBestelling);
            int amount = 0;
            int amouunt = listproducts.GroupBy(n => n.Id).Count();

            foreach(Product prod in listproducts)
            {
                
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
                    AvailableProducts = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);

                    List<Product> part1 = AvailableProducts.Take(3).ToList();
                    PriorityProducts = new ObservableCollection<Product>(part1);

                    int count = AvailableProducts.Count() - 3;
                    List<Product> part2 = AvailableProducts.Skip(3).Take(count).ToList();
                    OtherProducts = new ObservableCollection<Product>(part2);
                }
            }
        }

        public async void UpdateSaldo()
        {
            OpslaanBestelling();

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                KaartVM.LoggedInCostumer.Balance -= Totaal;
                string costumer = JsonConvert.SerializeObject(KaartVM.LoggedInCostumer);

                HttpResponseMessage response = await client.PutAsync("http://localhost:1428/api/costumer", new StringContent(costumer, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Update Balance");

                    Totaal = 0;
                    ProductsBestelling = null;
                }
            }
        }
    }
}
