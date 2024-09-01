using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Hurtownia.BusinessLogic
{
    public class ProductVM : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }
        public ICommand CheckoutCommand { get; }

        public ProductVM()
        {
            Products = new ObservableCollection<Product>
            {
                new Product { Name = "Product 1", Quantity = 0 },
                new Product { Name = "Product 2", Quantity = 0 },
                new Product { Name = "Product 3", Quantity = 0 },
                new Product { Name = "Product 4", Quantity = 0 },
                new Product { Name = "Product 5", Quantity = 0 },
                new Product { Name = "Product 6", Quantity = 0 },
                new Product { Name = "Product 7", Quantity = 0 },
                new Product { Name = "Product 8", Quantity = 0 }
            };

            CheckoutCommand = new RelayCommand(o => Checkout(), o => Products.Count > 0);
        }

        private void Checkout()
        {
            foreach (var product in Products)
            {
                product.Quantity = 0;  // Reset quantity for each product
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
