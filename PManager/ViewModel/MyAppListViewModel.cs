using PManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PManager.Repositorios;

namespace PManager.ViewModel
{
    public class MyAppListViewModel:BaseViewModel
    {
        private ObservableCollection<ExpanderViewModel> _items;
        private AppRepositable appRepo;

        public ObservableCollection<ExpanderViewModel> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }
        
        public MyAppListViewModel()
        {
            appRepo= new AppRepo();
            List<AppModel>appList=appRepo.UpdateData();
            List<string> appNames = new List<string>();

            Items = new ObservableCollection<ExpanderViewModel>();
            ObservableCollection<ContentViewModel> ContentList = new ObservableCollection<ContentViewModel>();

            foreach (var appRegister in appList)
            {
                ContentList = new ObservableCollection<ContentViewModel>();

                foreach (var item in appRepo.GetContentList(appRegister.AppName))
                {

                    ContentList.Add(new ContentViewModel { UserAppName = item.UserAppName, UserAppPassword = item.UserAppPassword});

                }

                if (!appNames.Contains(appRegister.AppName)){

                    Items.Add(new ExpanderViewModel { AppName = appRegister.AppName, UsersNumber = appRegister.UsersNumber, PasswordsNumber = appRegister.PasswordsNumber, ItemsContent = ContentList });
                    appNames.Add(appRegister.AppName);
                }

            }
        }

    }
}
