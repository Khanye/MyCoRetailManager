using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDataManager.Library.Models
{
    public class ProductModel
    {
        public int Id { get; set; } 
        /// add a null check
        public string PrdctTitle { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public int QuantityInStock  { get; set; }
        public bool IsTaxable { get; set; }
    }
}
