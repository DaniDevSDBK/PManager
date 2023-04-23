using PManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PManager.ViewModel
{
    public class RegisterViewModel
    {

        private string userName;
        private SecureString password;
        private string errorMessage;
        private bool isViewVisible = true;
        private UIRepositable uRepos;

        public string UserName { get => userName; set => userName = value; }
        public SecureString Password { get => password; set => password = value; }
        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }
        public bool IsViewVisible { get => isViewVisible; set => isViewVisible = value; }
        public UIRepositable URepos { get => uRepos; set => uRepos = value; }


    }
}
