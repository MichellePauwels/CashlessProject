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
    public class KlantenVM : ObservableObject, IPage
    {
        //properties
        public string Name
        {
            get { return "Klanten"; }
        }

        private bool _isEditingCustomer;
        public bool IsEditingCustomer
        {
            get { return _isEditingCustomer; }
            set { _isEditingCustomer = value; OnPropertyChanged("IsEditingCustomer"); OnPropertyChanged("OverviewVisibility"); OnPropertyChanged("EditingCustomerVisibility"); }
        }
        

        private ObservableCollection<Costumer> _costumers;
        public ObservableCollection<Costumer> Costumers
        {
            get { return _costumers; }
            set { _costumers = value; OnPropertyChanged("Costumers"); } //onproperty kan pas als je : ObservableObject
        }

        private Costumer _selectedCostumer;
        public Costumer SelectedCostumer
        {
            get { return _selectedCostumer; }
            set { _selectedCostumer = value; OnPropertyChanged("SelectedCostumer"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        //constructor
        public KlantenVM()
        {
            if(Costumers == null)
            {
                GetCostumers();
            }
        }

        //visibilities
        public Visibility OverviewVisibility
        {
            get { if (!IsEditingCustomer) return Visibility.Visible; return Visibility.Collapsed; }
        }

        public Visibility EditingCustomerVisibility
        {
            get { if (!IsEditingCustomer) return Visibility.Collapsed; return Visibility.Visible; }
        }

        //commands
        public ICommand ChangeEditPage
        {
            get { return new RelayCommand(CheckCommand); }
        }

        public ICommand UpdateKlant
        {
            get { return new RelayCommand(CheckKlant); }
        }

        //methods
        public void CheckCommand()
        {
            if(SelectedCostumer != null)
            {
                IsEditingCustomer = true;
            }
        }

        public void CheckKlant()
        {
            if (SelectedCostumer != null && SelectedCostumer.CostumerName != null && SelectedCostumer.Address != null)
            {
                string value = SelectedCostumer.Balance.ToString();
                double balance;

                if(Double.TryParse(value, out balance))
                {
                    UpdateCostumer(SelectedCostumer);
                    IsEditingCustomer = false;
                }
            }
            else
            {
                Error = "Alle velden zijn verplicht!";
            }
        }

        //json
        public async void GetCostumers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/costumer");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Costumers = JsonConvert.DeserializeObject<ObservableCollection<Costumer>>(json);
                }
            }
        }

        public async void UpdateCostumer(Costumer UpdatedCost)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string costumer = JsonConvert.SerializeObject(UpdatedCost);

                HttpResponseMessage response = await client.PutAsync("http://localhost:1428/api/costumer", new StringContent(costumer, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Update Costumer");
                }
            }
        }
    }
}
