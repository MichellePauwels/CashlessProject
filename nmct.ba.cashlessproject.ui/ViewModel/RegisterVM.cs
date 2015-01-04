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
    public class RegisterVM : ObservableObject, IPage
    {
        //properties
        public string Name
        {
            get { return "Kassa"; }
        }

        private ObservableCollection<Register> _registers;
        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private Register _selectedRegister;
        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); CheckPage(); }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }      

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }

        private bool _isLinkingEmployee;
        public bool IslinkingEmployee
        {
            get { return _isLinkingEmployee; }
            set { _isLinkingEmployee = value; OnPropertyChanged("IslinkingEmployee"); OnPropertyChanged("OverviewVisibility"); OnPropertyChanged("LinkingEmployeeVisibility"); OnPropertyChanged("Employees"); }
        }
              
        //constructor
        public RegisterVM()
        {
            if(Registers == null)
            {
                GetRegisters();
            }
        }

        //visibilities
        public Visibility OverviewVisibility
        {
            get { if (!IslinkingEmployee) return Visibility.Visible; return Visibility.Collapsed; }
        }

        public Visibility LinkingEmployeeVisibility
        {
            get { if (!IslinkingEmployee) return Visibility.Collapsed; return Visibility.Visible; }
        }

        //commands 
        public ICommand MedewerkersLinkenPagina
        {
            get { return new RelayCommand(ToonMedewerkersLinkPagina); }
        }

        public ICommand LinkEmployeeCommand
        {
            get { return new RelayCommand(LinkEmployee); }
        }

        //methods
        public void CheckPage()
        {
            if(!IslinkingEmployee)
            {
                GetEmployeesById();
            }
            else
            {
                GetEmployees();
            }
        }

        public void ToonMedewerkersLinkPagina()
        {
            IslinkingEmployee = true;
        }

        public void LinkEmployee()
        {
            RegisterEmployee regemp = new RegisterEmployee();
            regemp.RegisterId = SelectedRegister.Id;
            regemp.EmployeeId = SelectedEmployee.Id;
            regemp.From = DateTime.Now;
            regemp.Until = DateTime.Now.AddYears(1);

            RegisterEmployeeLink(regemp);
        }

        //json
        public async void GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/register");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                }
            }
        }

        public async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/employee/");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        private async void GetEmployeesById()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/registeremployee/" + SelectedRegister.Id);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        public async void RegisterEmployeeLink(RegisterEmployee regemp)
        {
            using(HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string registeremployee = JsonConvert.SerializeObject(regemp);

                HttpResponseMessage response = await client.PostAsync("http://localhost:1428/api/registeremployee", new StringContent(registeremployee, Encoding.UTF8, "application/json"));
                if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success link employee to register");
                    IslinkingEmployee = false;
                }
            }
        }
    }
}
