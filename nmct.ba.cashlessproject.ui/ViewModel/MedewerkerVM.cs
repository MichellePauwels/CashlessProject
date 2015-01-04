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
    public class MedewerkerVM : ObservableObject, IPage
    {
        //properties
        private bool _isAddingMedewerker;
        public bool IsAddingMedewerker
        {
            get { return _isAddingMedewerker; }
            set { _isAddingMedewerker = value; OnPropertyChanged("IsAddingMedewerker"); OnPropertyChanged("OverviewVisibility"); OnPropertyChanged("AddingMedewerkerVisibility"); }
        }  

        public string Name
        {
            get { return "Medewerkers"; }
        }

        private ObservableCollection<Employee> _medewerkers;
        public ObservableCollection<Employee> Medewerkers
        {
            get { return _medewerkers; }
            set { _medewerkers = value; OnPropertyChanged("Medewerkers"); }
        }

        private Employee _selectedMedewerker;
        public Employee SelectedMedewerker
        {
            get { return _selectedMedewerker; }
            set { _selectedMedewerker = value; OnPropertyChanged("SelectedMedewerker"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        //contructor
        public MedewerkerVM()
        {
            if (Medewerkers == null)
            {
                GetMedewerkers();
            }
        }

        //visibilities
        public Visibility OverviewVisibility
        {
            get { if (!IsAddingMedewerker) return Visibility.Visible; return Visibility.Collapsed; }
        }

        public Visibility AddingMedewerkerVisibility
        {
            get { if (!IsAddingMedewerker) return Visibility.Collapsed; return Visibility.Visible; }
        }

        //commands
        public ICommand ChangeMedewerkerPage
        {
            get { return new RelayCommand(CheckCommand); }
        }

        public ICommand AddMedewerker
        {
            get { return new RelayCommand(CheckMedewerker); }
        }

        public ICommand DeleteMedewerkerCommand
        {
            get { return new RelayCommand(DeleteMedewerker); }
        }

        public ICommand EditMedewerker
        {
            get { return new RelayCommand(EditMed); }
        }

        //methods
        public void CheckCommand()
        {
            IsAddingMedewerker = true;

            Employee e = new Employee();
            Medewerkers.Add(e);
            SelectedMedewerker = e;
        }

        public void CheckMedewerker()
        {
            if (SelectedMedewerker.Id == 0)
            {
                if (SelectedMedewerker.EmployeeName != null && SelectedMedewerker.Address != null && SelectedMedewerker.Email != null && SelectedMedewerker.Phone != null)
                {
                    if(SelectedMedewerker.Phone.Length >= 10)
                    {
                        AddNewMedewerker(SelectedMedewerker);
                        IsAddingMedewerker = false;
                    }
                    else
                    {
                        Error = "Telefoonnummer mag niet langer dan 10 cijfers zijn!";
                    }
                }
                else
                {
                    Error = "Alle velden zijn verplicht!";
                }
            }
            else
            {
                if (SelectedMedewerker != null && SelectedMedewerker.EmployeeName != null && SelectedMedewerker.Address != null && SelectedMedewerker.Email != null && SelectedMedewerker.Phone != null)
                {
                    UpdateMedewerker(SelectedMedewerker);
                    IsAddingMedewerker = false;
                }
                else
                {
                    Error = "Alle velden zijn verplicht!";
                }
            }
        }

        public void EditMed()
        {
            if (SelectedMedewerker != null)
            {
                IsAddingMedewerker = true;
            }
        }

        //json
        private async void GetMedewerkers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:1428/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Medewerkers = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        public async void AddNewMedewerker(Employee NewEmployee)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string medewerker = JsonConvert.SerializeObject(NewEmployee);

                HttpResponseMessage response = await client.PostAsync("http://localhost:1428/api/employee", new StringContent(medewerker, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Add Product");
                }
            }
        }

        public async void DeleteMedewerker()
        {
            if (SelectedMedewerker != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync("http://localhost:1428/api/employee/" + SelectedMedewerker.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        Medewerkers.Remove(SelectedMedewerker);
                    }
                    else
                    {
                        Console.WriteLine("Delete Error");
                    }
                }
            }
        }

        public async void UpdateMedewerker(Employee UpdatedMed)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string medewerker = JsonConvert.SerializeObject(UpdatedMed);

                HttpResponseMessage response = await client.PutAsync("http://localhost:1428/api/employee", new StringContent(medewerker, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succes Update Product");
                }
            }
        }
    }
}
