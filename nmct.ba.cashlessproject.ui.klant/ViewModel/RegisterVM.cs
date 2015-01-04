using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Model.Costumer;
using nmct.ba.cashlessproject.ui.klant.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.klant.ViewModel
{
    public class RegisterVM : ObservableObject, IPage
    {
        //properties
        public string Name
        {
            get { return "Registreren"; }
        }

        public EID EIDCostumer { get; set; }

        public ApplicationVM Appvm { get; set; }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }

        private Costumer _loggedInCostumer;
        public Costumer LoggedInCostumer
        {
            get { return _loggedInCostumer; }
            set { _loggedInCostumer = value; }
        }      
        
        //contructor
        public RegisterVM()
        {
            LaadEID();
        }

        //commands
        public ICommand Registreren
        {
            get { return new RelayCommand(CheckKlant); }
        }

        //methods
        private void LaadEID()
        {
            EIDReader eidreader = new EIDReader();
            EIDCostumer = EIDReader.Init();

            if(EIDCostumer == null)
            {
                Error = "Gelieve je kaart in te steken en de applicatie te herstarten!";
            }
        }

        private void CheckKlant()
        {
            if(EIDCostumer != null)
            {
                Costumer costumerOnCard = new Costumer();
                costumerOnCard.Rijksregisternummer = EIDCostumer.Rijksregisternummer;
                costumerOnCard.CostumerName = EIDCostumer.Firstname + " " + EIDCostumer.Surname;
                costumerOnCard.Address = EIDCostumer.Street + " " + EIDCostumer.Country;

                //vereniging laten inloggen en dan zo met de claims doen
                LoginVereniging();

                GetCostumers(costumerOnCard);
            }
        }

        public void LoginVereniging()
        {
            Appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();
        }

        //json
        public async void GetCostumers(Costumer cost)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);

                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/costumer/");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    ObservableCollection<Costumer> costumers = new ObservableCollection<Costumer>();
                    costumers = JsonConvert.DeserializeObject<ObservableCollection<Costumer>>(json);

                    bool isInDatabase = true;

                    foreach (Costumer costumer in costumers)
                    {
                        if (costumer.Rijksregisternummer == cost.Rijksregisternummer)
                        {
                            isInDatabase = true;

                            LoggedInCostumer = new Costumer();
                            LoggedInCostumer.CostumerName = costumer.CostumerName;
                            LoggedInCostumer.Address = costumer.Address;
                            LoggedInCostumer.Rijksregisternummer = costumer.Rijksregisternummer;
                            LoggedInCostumer.Balance = costumer.Balance;
                            LoggedInCostumer.Id = costumer.Id;

                            break;
                        }
                        else
                        {
                            isInDatabase = false;
                        }
                    }

                    if (!isInDatabase)
                    {
                        AddNewCostumer(cost);
                    }                   
                    else
                    {
                        Console.WriteLine("Succes login costumer");
                        Appvm.ChangePage(new ContainerVM(LoggedInCostumer));
                    }            
                }       
            }
        }

        public async void AddNewCostumer(Costumer cost)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string costumer = JsonConvert.SerializeObject(cost);

                HttpResponseMessage response = await client.PostAsync("http://localhost:1428/api/costumer", new StringContent(costumer, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes add new costumer");

                    string json = await response.Content.ReadAsStringAsync();
                    int newCostumerId = JsonConvert.DeserializeObject<int>(json);

                    GetCostumerById(newCostumerId);   
                }
            }
        }

        public async void GetCostumerById(int idCostumer)
        {
            using(HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/costumer/" + idCostumer);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes login new costumer");

                    string json = await response.Content.ReadAsStringAsync();
                    Costumer newCostumer = JsonConvert.DeserializeObject<Costumer>(json);

                    LoggedInCostumer = new Costumer();
                    LoggedInCostumer.CostumerName = newCostumer.CostumerName;
                    LoggedInCostumer.Address = newCostumer.Address;
                    LoggedInCostumer.Rijksregisternummer = newCostumer.Rijksregisternummer;
                    LoggedInCostumer.Balance = newCostumer.Balance;
                    LoggedInCostumer.Id = newCostumer.Id;

                    Appvm.ChangePage(new ContainerVM(LoggedInCostumer));
                }
            }
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:1428/token"));
            return client.RequestResourceOwnerPasswordAsync("KVKKortrijk", "kvk").Result;
        }
    }
}
