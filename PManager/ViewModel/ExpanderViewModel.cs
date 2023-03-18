using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.ViewModel
{
    public class ExpanderViewModel:BaseViewModel
    { 
        private string _AppName;
        private int _UsersNumber;
        private int _PasswordsNumber;
        private ObservableCollection<ContentViewModel> _itemsContent;

        public string AppName
        {
            get { return _AppName; }
            set { _AppName = value; OnPropertyChanged("AppName"); }
        }

        public int UsersNumber
        {
            get { return _UsersNumber; }
            set { _UsersNumber = value; OnPropertyChanged("UsersNumber"); }
        }

        public int PasswordsNumber
        {
            get { return _PasswordsNumber; }
            set { _PasswordsNumber = value; OnPropertyChanged("PasswordsNumber"); }
        }

        public ObservableCollection<ContentViewModel> ItemsContent
        {
            get { return _itemsContent; }
            set { _itemsContent = value; OnPropertyChanged("ItemsContent"); }
        }

    }
}
