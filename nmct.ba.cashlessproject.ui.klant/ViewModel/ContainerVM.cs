using GalaSoft.MvvmLight.CommandWpf;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.klant.ViewModel
{
    public class ContainerVM : ObservableObject, IPage
    {
        public static TokenResponse token = null;

        public ContainerVM(Costumer loggedInCostumer)
        {
            Pages.Add(new SaldoVM(loggedInCostumer));

            CurrentPage = Pages[0];
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
