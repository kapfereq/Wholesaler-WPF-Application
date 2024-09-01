using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Hurtownia.BusinessLogic
{
    public class Product : INotifyPropertyChanged
    {
        private int _quantity;
        public string Name { get; set; }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public Product()
        {
            AddCommand = new RelayCommand(o => Quantity++, o => true);
            RemoveCommand = new RelayCommand(o =>
            {
                if (Quantity > 0) Quantity--;
            }, o => Quantity > 0);  // Fixing the lambda expression here
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
