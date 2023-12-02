using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library
{
    public class ConfigHelper 
    {
        public static decimal GetTaxRate()
        {
            string taxRateText = ConfigurationManager.AppSettings["taxRate"];
            bool IsValidTaxRate = Decimal.TryParse(taxRateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }
            return output;
        }
    }
}
