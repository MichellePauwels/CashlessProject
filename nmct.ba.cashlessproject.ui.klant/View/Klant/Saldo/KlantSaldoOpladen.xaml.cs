using nmct.ba.cashlessproject.ui.klant.ViewModel;
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

namespace nmct.ba.cashlessproject.ui.klant.View.Klant.Saldo
{
    /// <summary>
    /// Interaction logic for KlantSaldoOpladen.xaml
    /// </summary>
    public partial class KlantSaldoOpladen : UserControl
    {
        public KlantSaldoOpladen()
        {
            InitializeComponent();
        }

        private void txbFiveEuro_GotFocus(object sender, RoutedEventArgs e)
        {
            SaldoVM.SelectedTextBox = "TxbFiveEuro";
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SaldoVM.SelectedTextBox = "TxbTenEuro";
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            SaldoVM.SelectedTextBox = "TxbTwentyEuro";
        }

        private void TextBox_GotFocus_2(object sender, RoutedEventArgs e)
        {
            SaldoVM.SelectedTextBox = "TxbFiftyEuro";
        }
    }
}
