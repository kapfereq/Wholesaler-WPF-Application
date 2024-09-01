using System.Windows.Controls;
using Hurtownia.BusinessLogic;

namespace HurtowniaAplikacja
{
    public partial class Products : UserControl
    {
        public Products()
        {
            InitializeComponent();
            // DataContext = new ProductVM();  // Ensure DataContext is set if not in XAML
        }
    }
}
