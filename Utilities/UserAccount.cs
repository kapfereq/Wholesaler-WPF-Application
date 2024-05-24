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
    public class UserAccount
    {
        public string Username { get; set; }
        public string Password { get; set; } // **Security Risk**
    }

    public class AccountManager
    {
        private readonly string _userAccountsFile = "HurtowniaAplikacja;component/UserAccounts/UserAccounts.txt";
        public void CreateAccount(string username, SecureString password)
        {
            // Validate username (optional, but recommended)
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            // Validate password (now required)
            if (password == null || password.Length == 0) // Check for null and empty password
            {
                MessageBox.Show("Please enter a password. Password cannot be empty.");
                return;
            }

            // Convert SecureString to plain text (NOT RECOMMENDED, for demonstration only)
            string passwordText = Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password));
            Marshal.FreeBSTR(Marshal.SecureStringToBSTR(password)); // Clear SecureString

            // Create the UserAccount object with plain text password
            var userAccount = new UserAccount { Username = username, Password = passwordText };

            // Save the user account (assuming uniqueness is checked here and SaveUserAccount handles potential file access exceptions)
            SaveUserAccount(userAccount);
        }



        public UserAccount TryLogin(string username, SecureString password)
        {
            // Get the user accounts folder path (assuming it exists)
            string userAccountsFolder = Path.Combine(Environment.CurrentDirectory, "UserAccounts");
            string userAccountsFile = Path.Combine(userAccountsFolder, "UserAccounts.txt");

            if (!File.Exists(userAccountsFile))
            {
                MessageBox.Show("No user accounts file found!", "Login Error");
                return null;
            }

            // Check for null password and display message box if needed
            if (password == null)
            {
                MessageBox.Show("Please enter your password!", "Login Error");
                return null; // Login failed due to missing password
            }

            // Convert SecureString to plain text (not recommended for production)
            string enteredPasswordText = Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password));
            Marshal.FreeBSTR(Marshal.SecureStringToBSTR(password)); // Clear SecureString

            // Read all lines from the user accounts file
            string[] lines = File.ReadAllLines(userAccountsFile);

            // Check if any line matches the username and password exactly
            bool isLoginSuccessful = lines.Any(line => line == $"{username}:{enteredPasswordText}");

            if (isLoginSuccessful)
            {
                MessageBox.Show("Login successful!");
                return new UserAccount { Username = username };
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Login Error");
            }

            return null;
        }




        private List<UserAccount> LoadUserAccounts()
        {
            List<UserAccount> accounts = new List<UserAccount>();

            if (!File.Exists(_userAccountsFile))
            {
                return accounts; // No user accounts file
            }

            string[] lines = File.ReadAllLines(_userAccountsFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length != 2)
                {
                    continue; // Invalid format in user accounts file
                }

                accounts.Add(new UserAccount { Username = parts[0], Password = parts[1] });
            }

            return accounts;
        }


        private void SaveUserAccount(UserAccount account)
        {
            
            string userAccountsFolder = Path.Combine(Environment.CurrentDirectory, "UserAccounts");

            string userAccountsFile = Path.Combine(userAccountsFolder, "UserAccounts.txt");

            Console.WriteLine($"User Accounts File Path (Debug): {userAccountsFile}");

            if (!IsUsernameUnique(account.Username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return; 
            }

            if (!Directory.Exists(userAccountsFolder))
            {
                Directory.CreateDirectory(userAccountsFolder);
            }

            try
            {
                using (StreamWriter writer = File.AppendText(userAccountsFile))
                {
                    string line = $"{account.Username}:{account.Password}";

                    writer.WriteLine(line);
                }

                // Success message 
                MessageBox.Show("User account saved successfully!");
                var window = Application.Current.MainWindow as MainLoginWindow; // Get reference
                if (window != null)
                {
                    window.MyContentControl.ContentTemplate = window.Resources["LoginTemplate"] as DataTemplate;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error saving user account: {ex.Message}");
            }
        }

        private bool IsUsernameUnique(string username)
        {
            // Get the user accounts folder path (assuming it exists)
            string userAccountsFolder = Path.Combine(Environment.CurrentDirectory, "UserAccounts");
            string userAccountsFile = Path.Combine(userAccountsFolder, "UserAccounts.txt");

            //Console.WriteLine($"Checking Username (Debug): {username}"); // Debug Print - Username Verification

            if (string.IsNullOrEmpty(username)) 
            {
                return false;
            }

            // Read all lines from the user accounts file
            if (!Directory.Exists(userAccountsFolder))
            {
                Directory.CreateDirectory(userAccountsFolder);
            }
            if (!File.Exists(userAccountsFile))
            {
                // Create an empty file
                File.Create(userAccountsFile).Dispose();
                // Now the code can continue (or handle the empty file scenario)
            }
            string[] lines = File.ReadAllLines(userAccountsFile);

            // Check if any line starts with the username and a colon (:)
            return !lines.Any(line => line.StartsWith(username + ":"));
        }
    }
}
