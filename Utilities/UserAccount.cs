using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; // For Marshal class
using System.IO; // For File class
using System.Windows;

namespace HurtowniaAplikacja
{
    public interface IUserAccount
    {
        string Username { get; set; }
        string Password { get; set; }
    }

    public interface IAccountManager
    {
        void CreateAccount(string username, SecureString password);
        IUserAccount TryLogin(string username, SecureString password);
    }

    public interface IFileHandler
    {
        bool FileExists(string path);
        void CreateDirectory(string path);
        void CreateFile(string path);
        string[] ReadAllLines(string path);
        void AppendTextToFile(string path, string content);
    }

    public class StandardUserAccount : IUserAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminUserAccount : IUserAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void AccessAdminPanel()
        {
            MessageBox.Show("Accessing Admin Panel");
        }
    }

    public class FileHandler : IFileHandler
    {
        public bool FileExists(string path) => File.Exists(path);

        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void CreateFile(string path)
        {
            if (!FileExists(path))
                File.Create(path).Dispose();
        }

        public string[] ReadAllLines(string path) => File.ReadAllLines(path);

        public void AppendTextToFile(string path, string content)
        {
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine(content);
            }
        }
    }

    public class AccountManager : IAccountManager
    {
        private readonly string _userAccountsFile = "HurtowniaAplikacja;component/UserAccounts/UserAccounts.txt";
        private readonly IFileHandler _fileHandler;

        public AccountManager()
        {
            _fileHandler = new FileHandler(); // Directly instantiate FileHandler
        }

        public void CreateAccount(string username, SecureString password)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            if (password == null || password.Length == 0)
            {
                MessageBox.Show("Please enter a password. Password cannot be empty.");
                return;
            }

            string passwordText = Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password));
            Marshal.FreeBSTR(Marshal.SecureStringToBSTR(password));

            IUserAccount userAccount;

            // Check if the username starts with "ADMIN_"
            if (username.StartsWith("ADMIN_"))
            {
                userAccount = new AdminUserAccount { Username = username, Password = passwordText };
            }
            else
            {
                userAccount = new StandardUserAccount { Username = username, Password = passwordText };
            }

            SaveUserAccount(userAccount);
        }

        public IUserAccount TryLogin(string username, SecureString password)
        {
            string userAccountsFolder = Path.Combine(Environment.CurrentDirectory, "UserAccounts");
            string userAccountsFile = Path.Combine(userAccountsFolder, "UserAccounts.txt");

            if (!_fileHandler.FileExists(userAccountsFile))
            {
                MessageBox.Show("No user accounts file found!", "Login Error");
                return null;
            }

            if (password == null)
            {
                MessageBox.Show("Please enter your password!", "Login Error");
                return null;
            }

            string enteredPasswordText = Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password));
            Marshal.FreeBSTR(Marshal.SecureStringToBSTR(password));

            string[] lines = _fileHandler.ReadAllLines(userAccountsFile);

            bool isLoginSuccessful = lines.Any(line => line == $"{username}:{enteredPasswordText}");

            if (isLoginSuccessful)
            {
                // Check if the username starts with "ADMIN_"
                if (username.StartsWith("ADMIN_"))
                {
                    MessageBox.Show("Admin account logged in successfully");
                    return new AdminUserAccount { Username = username, Password = enteredPasswordText };
                }
                else
                {
                    MessageBox.Show("Login successful!");
                    return new StandardUserAccount { Username = username, Password = enteredPasswordText };
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Login Error");
            }

            return null;
        }

        private List<IUserAccount> LoadUserAccounts()
        {
            List<IUserAccount> accounts = new List<IUserAccount>();

            if (!_fileHandler.FileExists(_userAccountsFile))
            {
                return accounts;
            }

            string[] lines = _fileHandler.ReadAllLines(_userAccountsFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length != 2)
                {
                    continue;
                }

                // Check if the username starts with "ADMIN_"
                if (parts[0].StartsWith("ADMIN_"))
                {
                    accounts.Add(new AdminUserAccount { Username = parts[0], Password = parts[1] });
                }
                else
                {
                    accounts.Add(new StandardUserAccount { Username = parts[0], Password = parts[1] });
                }
            }

            return accounts;
        }

        private void SaveUserAccount(IUserAccount account)
        {
            string userAccountsFolder = Path.Combine(Environment.CurrentDirectory, "UserAccounts");
            string userAccountsFile = Path.Combine(userAccountsFolder, "UserAccounts.txt");

            if (!IsUsernameUnique(account.Username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return;
            }

            _fileHandler.CreateDirectory(userAccountsFolder);
            _fileHandler.AppendTextToFile(userAccountsFile, $"{account.Username}:{account.Password}");

            MessageBox.Show("User account saved successfully!");
            var window = Application.Current.MainWindow as MainLoginWindow;
            if (window != null)
            {
                window.MyContentControl.ContentTemplate = window.Resources["LoginTemplate"] as DataTemplate;
            }
        }

        private bool IsUsernameUnique(string username)
        {
            string userAccountsFolder = Path.Combine(Environment.CurrentDirectory, "UserAccounts");
            string userAccountsFile = Path.Combine(userAccountsFolder, "UserAccounts.txt");

            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            _fileHandler.CreateDirectory(userAccountsFolder);
            if (!_fileHandler.FileExists(userAccountsFile))
            {
                _fileHandler.CreateFile(userAccountsFile);
            }

            string[] lines = _fileHandler.ReadAllLines(userAccountsFile);
            return !lines.Any(line => line.StartsWith(username + ":"));
        }
    }
}
