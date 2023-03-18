using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.ViewModel
{
    public class ContentViewModel:BaseViewModel
    {

        private string _userAppName;
        private string _userAppPassword;

        public string UserAppName {
            get { return _userAppName; }
            set { _userAppName = value; OnPropertyChanged("UserAppName"); }
        }public string UserAppPassword {
            get { return _userAppPassword; }
            set { _userAppPassword = value; OnPropertyChanged("UserAppPassword"); }
        }
    }
}
