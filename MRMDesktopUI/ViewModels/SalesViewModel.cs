using AutoMapper;
using Caliburn.Micro;
using MRMDesktopUI.Library.Api;
using MRMDesktopUI.Library.Helpers;
using MRMDesktopUI.Library.Models;
using MRMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private IConfigHelper _configHelper;
        private ISaleEndpoint _saleEndpoint;
        private IMapper _mapper;
        public SalesViewModel(
            IProductEndpoint productEndpoint, 
            IConfigHelper configHelper, 
            ISaleEndpoint saleEndpoint,
            IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            // Couldnt work constructor does return anything, no return type so we cant make it asynchronous 

            //var productList = _productEndpoint.GetAll();
            //Products = new BindingList<ProductDisplayModel>(_productEndpoint.GetAll);
        }

        // overide onViewLOadedd to call LoadProducts
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        public async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();

            var pro = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(pro);
        }


        private BindingList<ProductDisplayModel> _products;
		public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set 
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
            }
		}


        private ProductDisplayModel _selectedProduct;
        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            //TODO - Clear out SelectedCartItem if its not clearin on its own
            await LoadProducts();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }     

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);

            }
        }


        private CartItemDisplayModel _selectedCartItem;
        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        private int _itemQuantity = 1;
		public int ItemQuantity 
		{
			get { return _itemQuantity; }
			set 
			{
				_itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
		}

        public string SubTotal
        {
            get 
            {
                //TODO - Replace with calculation
                return CalcSubTotal().ToString("C");
            }
       }
        private decimal CalcSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in _cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }
        public string Tax
        {
            get
            {
                //TODO - Replace with calculation
                return CalculateTax().ToString("C");
            }
        }
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;

            // Improving the for each loop with an if inside
            taxAmount =  Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice *  x.QuantityInCart * taxRate);

            //foreach (var item in _cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate );
            //    }
            //}
            return taxAmount;
        }
        public string Total
        {
            get
            {
                //TODO - Replace with calculation
                decimal total = CalcSubTotal() + CalculateTax(); 
                return total.ToString("C");
            }
        }
        public bool CanAddToCart
        {
            get
            {
                bool output = false;
				//Make sure something is selected 
				// Make sure that there is an item quantity
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }
                return output;
            }
        }
        public void AddToCart()
		{
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                ////This is a Hack improve it
                //Cart.Remove(existingItem);
                //Cart.Add(existingItem);
            }
            else
            {
                CartItemDisplayModel model = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(model);
            }           
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Cart);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                //Make sure something is selected 
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    output = true;
                }
                return output;
            }
        }
        public void RemoveFromCart()
        {
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
                SelectedCartItem.Product.QuantityInStock += 1;
            }
            else
            {
                SelectedCartItem.Product.QuantityInStock += 1;
                Cart.Remove(SelectedCartItem);
            }
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Cart);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }
        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                //Make sure something in the cart
                if (Cart.Count > 0)
                {
                    output = true;
                }
                return output;
            }
        }
        public async Task CheckOut()
        {
            // Create a SaleModel and post to the API
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductID = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);

            await ResetSalesViewModel();
        }

    }
}
