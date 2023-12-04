using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDesktopUI.Models
{
    public class CartItemDisplayModel: INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }

        private int _quantityInCart;
        public int QuantityInCart  
        {
            get { return _quantityInCart; }
            set 
            {
                _quantityInCart = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantityInCart)));
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(DisplayText);
            }
        }
        public string DisplayText
        {
            get
            {
                return $"{Product.PrdctTitle}  ({QuantityInCart})";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
