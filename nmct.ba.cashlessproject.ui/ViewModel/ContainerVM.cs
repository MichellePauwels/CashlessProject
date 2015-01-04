using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class ContainerVM : ObservableObject, IPage
    {
        public static TokenResponse token = null;

        public ContainerVM()
        {
            //beter met id
            //this werkt niet voor static fields

            Pages.Add(new ProductVM());
            Pages.Add(new KlantenVM());
            Pages.Add(new MedewerkerVM());
            Pages.Add(new RegisterVM());
            Pages.Add(new AccountVM());

            CurrentPage = Pages[0];
        }

        private static string _loggedInUserId;
        public static string LoggedInId
        {
            get { return _loggedInUserId; }
            set { _loggedInUserId = value; }
        }   

        private object _currentPage;
        public object CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private List<IPage> _pages;
        public List<IPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<IPage>();
                return _pages;
            }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        public void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        public string Name
        {
            get { return "Container"; }
        }
    }
}
