﻿using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Security;

namespace HurtowniaAplikacja
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainLoginWindow : Window, INotifyPropertyChanged
    {
        private readonly AccountManager _accountManager = new AccountManager();
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
        public SecureString SecurePassword { private get; set; }
        public MainLoginWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void MainLoginWindow_OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (_accountManager.TryLogin(LoginUsername, SecurePassword))
            {
                
            }
            else
            {
                
            }
        }
        private void MainLoginWindow_OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            _accountManager.CreateAccount(LoginUsername, SecurePassword);
        }
        public void changeTemplate(object sender, RoutedEventArgs e)
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
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
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
