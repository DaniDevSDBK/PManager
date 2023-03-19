using PManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PManager.ViewModel
{
    public class SettingsViewModel:BaseViewModel
    {

        private UserAccountModel _currentUserAccount;

        public UserAccountModel CurrentUserAccount { get { return _currentUserAccount; } }

        public SettingsViewModel()
        {

            var mainWindow = (MainWindow)System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            var mainWindowViewModel = (MainViewModel)mainWindow.DataContext;

            _currentUserAccount = mainWindowViewModel.CurrentUserAccount;

        }


    }
}
