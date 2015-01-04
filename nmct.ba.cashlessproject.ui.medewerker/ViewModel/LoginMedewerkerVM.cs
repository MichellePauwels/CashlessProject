using GalaSoft.MvvmLight.CommandWpf;
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
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.medewerker.ViewModel
{
    public class LoginMedewerkerVM : ObservableObject, IPage
    {
        //properties
        public string Name
        {
            get { return "Login"; }
        }

        public ApplicationVM Appvm { get; set; }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        private ObservableCollection<Employee> _medewerkers;
        public ObservableCollection<Employee> Medewerkers
        {
            get { return _medewerkers; }
            set { _medewerkers = value; OnPropertyChanged("Medewerkers"); }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; }
        }
         
        //constructor
        public LoginMedewerkerVM()
        {
            LoginVereniging();

            if (Medewerkers == null)
            {
                GetMedewerkers();
            }
        }

        //commands
        public ICommand AddNumberCommand
        {
            get { return new RelayCommand<object>(UpdatePhoneNumber); }
        }

        public ICommand LoginCommand
        {
            get { return new RelayCommand(CheckEmployee); }
        }

        //methods
        public void UpdatePhoneNumber(object numberBtn)
        {
            string number = numberBtn as string;
            int phonenumberDigits;

            if(PhoneNumber != null)
            {
                phonenumberDigits = PhoneNumber.Length;
            }
            else
            {
                phonenumberDigits = 0;
            }

            if(phonenumberDigits >= 10)
            {
                Error = "Telefoonnummer mag niet langer dan 10 cijfers zijn!";

                if(number == "×")
                {
                    PhoneNumber = PhoneNumber.Remove(PhoneNumber.Length - 1);
                    Error = "";
                }
            }
            else
            {
                switch (number)
                {
                    case "0":
                        PhoneNumber += 0;
                        break;

                    case "1":
                        PhoneNumber += 1;
                        break;

                    case "2":
                        PhoneNumber += 2;
                        break;

                    case "3":
                        PhoneNumber += 3;
                        break;

                    case "4":
                        PhoneNumber += 4;
                        break;

                    case "5":
                        PhoneNumber += 5;
                        break;

                    case "6":
                        PhoneNumber += 6;
                        break;

                    case "7":
                        PhoneNumber += 7;
                        break;

                    case "8":
                        PhoneNumber += 8;
                        break;

                    case "9":
                        PhoneNumber += 9;
                        break;

                    case "×":
                        PhoneNumber = PhoneNumber.Remove(PhoneNumber.Length - 1);
                        break;

                    default:
                        Error = "Er is iets misgelopen!";
                        break;
                }
            }
        }

        public void LoginVereniging()
        {
            Appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();
        }

        public void CheckEmployee()
        {
            if(SelectedEmployee != null)
            {
                if(SelectedEmployee.Phone == PhoneNumber)
                {
                    Login();
                }
                else
                {
                    Error = "Telefoonnummer is niet juist!";
                }
            }
            else
            {
                Error = "Gelieve je naam te kiezen!";
            }
        }

        public void Login()
        {
            Appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            Appvm.ChangePage(new ContainerVM());
        }

        //json
        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:1428/token"));
            return client.RequestResourceOwnerPasswordAsync("KVKKortrijk", "kvk").Result;
        }

        private async void GetMedewerkers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes login employee");

                    string json = await response.Content.ReadAsStringAsync();
                    Medewerkers = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }
    }
}
