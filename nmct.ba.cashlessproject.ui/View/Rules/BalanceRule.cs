using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace nmct.ba.cashlessproject.ui.View.Rules
{
    public class BalanceRule : ValidationRule
    {
        public double Balance { get; set; }
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double i;

            if (Double.TryParse((string)value, out i))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Het saldo moet een getal zijn!");
            }
        }
    }
}
