using GalaSoft.MvvmLight.CommandWpf;
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
    public class SaldoVM : ObservableObject, IPage
    {
        //properties
        public string Name
        {
            get { return "Saldo"; }
        }

        public EID EIDCostumer { get; set; }

        private Costumer _loggedInCostumer;
        public Costumer LoggedInCostumer
        {
            get { return _loggedInCostumer; }
            set { _loggedInCostumer = value; OnPropertyChanged("LoggedInCostumer"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        private int _fiveEuroBills;
        public int FiveEuroBills
        {
            get { return _fiveEuroBills; }
            set { _fiveEuroBills = value; OnPropertyChanged("FiveEuroBills"); UpdateTempSaldo("five"); OnPropertyChanged("Balance"); }
        }

        private int _tenEuroBills;
        public int TenEuroBills
        {
            get { return _tenEuroBills; }
            set { _tenEuroBills = value; OnPropertyChanged("TenEuroBills"); UpdateTempSaldo("ten"); OnPropertyChanged("Balance"); }
        }

        private int _twentyEuroBills;
        public int TwentyEuroBills
        {
            get { return _twentyEuroBills; }
            set { _twentyEuroBills = value; OnPropertyChanged("TwentyEuroBills"); UpdateTempSaldo("twenty"); OnPropertyChanged("Balance"); }
        }

        private int _fiftyEuroBills;
        public int FiftyEuroBills
        {
            get { return _fiftyEuroBills; }
            set { _fiftyEuroBills = value; OnPropertyChanged("FiftyEuroBills"); UpdateTempSaldo("fifty"); OnPropertyChanged("Balance"); }
        }

        private static string _selectedTextbox;
        public static string SelectedTextBox
        {
            get { return _selectedTextbox; }
            set { _selectedTextbox = value; }
        }
        
        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged("Balance"); }
        }
                
        //constructor
        public SaldoVM(Costumer loggedInCostumer)
        {
            LoggedInCostumer = loggedInCostumer;
            Balance = loggedInCostumer.Balance;
        }

        //commands
        public ICommand ResetBedragCommand
        {
            get { return new RelayCommand(ResetBedrag); }
        }

        public ICommand UpdateBalanceCommand
        {
            get { return new RelayCommand(CheckBalance); }
        }

        public ICommand AddNumberCommand
        {
            get { return new RelayCommand<object>(Test); }
        }

        public ICommand LogoffCommand
        {
            get { return new RelayCommand(Logoff); }
        }

        //methods 
        private void Logoff()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = null;

            appvm.ChangePage(new RegisterVM());
        }

        private void Test(object numberString)
        {
            int number = Convert.ToInt32(numberString);

            switch (SelectedTextBox)
            {
                case "TxbFiveEuro":
                    FiveEuroBills = number;
                break;

                case "TxbTenEuro":
                    TenEuroBills = number;
                break;

                case "TxbTwentyEuro":
                    TwentyEuroBills = number;
                break;

                case "TxbFiftyEuro":
                    FiftyEuroBills = number;
                break;

                default:
                    break;
            }
        }

        public void ResetBedrag()
        {
            OnPropertyChanged("FiveEuroBills");
            OnPropertyChanged("TenEuroBills");
            OnPropertyChanged("TwentyEuroBills");
            OnPropertyChanged("FiftyEuroBills");
            OnPropertyChanged("Balance");

            Balance = LoggedInCostumer.Balance;
            FiveEuroBills = 0;
            TenEuroBills = 0;
            TwentyEuroBills = 0;
            FiftyEuroBills = 0;
            Error = "";
        }

        private void CheckBalance()
        {
            double difference = Math.Abs(Balance - LoggedInCostumer.Balance);
            if(difference > 100)
            {
                Error = "Het op te laden bedrag mag niet meer dan 100 zijn!";
            }
            else
            {
                LoggedInCostumer.Balance = Balance;
                UpdateBedrag(LoggedInCostumer);
            }
        }

        private void UpdateTempSaldo(string updatedbill)
        {
            switch (updatedbill)
            {
                case "five":
                    Balance += (FiveEuroBills * 5);
                    break;

                case "ten":
                    Balance += (TenEuroBills * 10);
                    break;

                case "twenty":
                    Balance += (TwentyEuroBills * 20);
                    break;

                case "fifty":
                    Balance += (FiftyEuroBills * 50);
                    break;

                default:
                    break;
            }
        }

        public async void UpdateBedrag(Costumer UpdatedCost)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string costumer = JsonConvert.SerializeObject(UpdatedCost);

                HttpResponseMessage response = await client.PutAsync("http://localhost:1428/api/costumer", new StringContent(costumer, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Update Balance");
                    ResetBedrag();
                }
            }
        }
    }
}
