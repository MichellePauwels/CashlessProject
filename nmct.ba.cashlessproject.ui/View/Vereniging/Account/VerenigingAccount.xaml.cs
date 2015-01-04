using nmct.ba.cashlessproject.ui.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nmct.ba.cashlessproject.ui.View.Vereniging.Account
{
    /// <summary>
    /// Interaction logic for VerenigingAccount.xaml
    /// </summary>
    public partial class VerenigingAccount : UserControl
    {
        public VerenigingAccount()
        {
            InitializeComponent();
        }

        private void btnWijzigPwd2_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AccountVM).OldPassword = txtPwdOld.Password;
            (DataContext as AccountVM).NewPassword = txtPwdNew.Password;
        }
    }
}
