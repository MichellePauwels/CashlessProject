using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Model.Costumer;
using nmct.ba.cashlessproject.ui.medewerker.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.medewerker.ViewModel
{
    public class KaartVM : ObservableObject, IPage
    {
        //properties
        public string Name
        {
            get { return "Kaart"; }
        }

        private EID _eidCostumer;
	    public EID EIDCostumer
	    {
		    get { return _eidCostumer; }
            set { _eidCostumer = value; OnPropertyChanged("EIDCostumer"); }
	    }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        private static Costumer _loggedInCostumer;
        public static Costumer LoggedInCostumer
        {
            get { return _loggedInCostumer; }
            set { _loggedInCostumer = value; }
        }    
        
        //constructor
        public KaartVM()
        {
        }

        //commands 
        public ICommand GetCostumerCommand
        {
            get { return new RelayCommand(LaadEID); }
        }

        //methods
        private void LaadEID()
        {
            EIDReader eidreader = new EIDReader();
            EIDCostumer = EIDReader.Init();

            if (EIDCostumer == null)
            {
                Error = "Gelieve de kaart in te steken!";
            }
            else
            {
                Error = "";

                Costumer costumerOnCard = new Costumer();
                costumerOnCard.Rijksregisternummer = EIDCostumer.Rijksregisternummer;
                costumerOnCard.CostumerName = EIDCostumer.Firstname + " " + EIDCostumer.Surname;
                costumerOnCard.Address = EIDCostumer.Street + " " + EIDCostumer.Country;

                GetCostumers(costumerOnCard);
            }
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
                        LoggedInCostumer = null;

                        Error = "Deze klant is nog niet geregistreerd!";
                    }
                    else
                    {
                        ContainerVM appvm = new ContainerVM();
                        appvm.ChangePage(appvm.Pages[1]);
                    }
                }
            }
        }
    }
}
