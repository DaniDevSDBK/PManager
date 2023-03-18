using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Model
{
    public class AppModel
    {
        public string AppName { get; set; }
        public int AppId { get; set; }
        public int UserId { get; set; }
        public string AppPassword { get; set; }
        public string UserAppName { get; set; }
        public int UsersNumber { get; set; }
        public int PasswordsNumber { get; set; }

        public AppModel(){}
        public AppModel(string appName, string userAppName, string userAppPassword){ 
        
            AppName = appName;
            UserAppName= userAppName;
            AppPassword= userAppPassword;
        }

        public AppModel(string appName, int usersNumber, int passwordsNumber)
        {

            AppName = appName;
            UsersNumber = usersNumber;
            PasswordsNumber = passwordsNumber;
        }
    }
}
