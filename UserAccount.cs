using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HurtowniaAplikacja
{
    public class UserAccount
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public UserAccount(string username, string hashedPassword)
        {
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}
