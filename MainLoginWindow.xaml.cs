using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace HurtowniaAplikacja
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainLoginWindow : Window, INotifyPropertyChanged
    {
        private string CurrentTemplateName = "LoginTemplate";

        private string _loginUsername;
        public string LoginUsername
        {
            get { return _loginUsername; }
            set
            {
                _loginUsername = value;
                OnPropertyChanged("LoginUsername"); // Notify about property change
            }
        }
        string LoginPassword { get; set; }
        string RegisterUsername { get; set; }
        string RegisterPassword { get; set; }
        public MainLoginWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void MainLoginWindow_OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Login detected! " + LoginUsername);
        }
        private void MainLoginWindow_OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Register detected!");
        }
        private void changeTemplate(object sender, RoutedEventArgs e)
        {
            if (CurrentTemplateName == "LoginTemplate")
            {
                CurrentTemplateName = "RegistrationTemplate";
            }
            else
            {
                CurrentTemplateName = "LoginTemplate";
            }
            MyContentControl.ContentTemplate = (DataTemplate)Resources[CurrentTemplateName];
        }
        private void UsernameLogin_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            LoginUsername = ((TextBox)sender).Text;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
