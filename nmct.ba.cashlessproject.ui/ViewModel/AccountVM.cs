using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class AccountVM : ObservableObject, IPage
    {
        //properties
        private bool _isChangingPassword;
        public bool IsChangingPassword
        {
            get { return _isChangingPassword; }
            set { _isChangingPassword = value; OnPropertyChanged("IsChangingPassword"); OnPropertyChanged("OverviewVisibility"); OnPropertyChanged("ChangingPasswordVisibility"); }
        }

        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set { _oldPassword = value; }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; }
        }

        public string Name
        {
            get { return "Account"; }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }

        //visibilities
        public Visibility OverviewVisibility
        {
            get { if (!IsChangingPassword) return Visibility.Visible; return Visibility.Collapsed; }
        }

        public Visibility ChangingPasswordVisibility
        {
            get { if (!IsChangingPassword) return Visibility.Collapsed; return Visibility.Visible; }
        }

        //commands
        public ICommand ChangePasswordPage
        {
            get { return new RelayCommand(CheckCommand); }
        }

        public ICommand Afmelden
        {
            get { return new RelayCommand(Logoff); }
        }

        public ICommand EditPassword
        {
            get { return new RelayCommand(ChangePassword); }
        }

        //methods
        private void Logoff()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = null;

            appvm.ChangePage(new LoginVerenigingVM());
        }

        public void CheckCommand()
        {
            IsChangingPassword = true;
        }

        public void ChangePassword()
        {
            if(OldPassword != "" && NewPassword != "")
            {
                if(!ApplicationVM.token.IsError)
                {
                    UpdatePassword();
                    IsChangingPassword = false;
                }
                else
                {
                    Error = "Oud paswoord klopt niet!";
                }
            }
        }

        public async void UpdatePassword()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                Vereniging ver = new Vereniging();
                ver.Password = NewPassword;

                string vereniging = JsonConvert.SerializeObject(ver);

                HttpResponseMessage response = await client.PutAsync("http://localhost:1428/api/vereniging", new StringContent(vereniging, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("succes");
                }
            }
        }
    }
}
