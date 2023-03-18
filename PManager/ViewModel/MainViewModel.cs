using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FontAwesome.Sharp;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using Org.BouncyCastle.Utilities;
using PManager.Model;
using PManager.Repositorios;

namespace PManager.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private UserAccountModel currentUserAccount;
        private BaseViewModel currentChildView;
        private string _currentWindowTittle;
        private bool _isLoginVisible = false;
        private IconChar _iconView;
        private UIRepositable userRepo;
        private readonly IMessenger _messenger;
        public void ShowLoginView()
        {
            _messenger.Send(new ShowLogInViewMessage { Show = true });
        }
        public UserAccountModel CurrentUserAccount { get { return currentUserAccount; } set { currentUserAccount = value; OnPropertyChanged(nameof(CurrentUserAccount)); } }

        public string CurrentWindowTittle { get => _currentWindowTittle; set { _currentWindowTittle = value; OnPropertyChanged(nameof(CurrentWindowTittle)); } }
        public IconChar IconView { get => _iconView; set { _iconView = value; OnPropertyChanged(nameof(IconView)); } }

        //Propiedad ChildView
        public BaseViewModel CurrentChildView { get => currentChildView; set { currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); } }

        //Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowAddAppViewCommand { get; }
        public ICommand ShowMyAppListViewCommand { get; }
        public ICommand ShowSettingsViewCommand { get; }
        public ICommand ShowLogInViewCommand { get; }

        public MainViewModel() 
        { 
            userRepo = new UserRepo();
            

            //Initialize
            ShowHomeViewCommand = new RelayCommand(ExecuteShowHomeViewCommand);
            ShowAddAppViewCommand = new RelayCommand(ExecuteShowAddAppViewCommand);
            ShowMyAppListViewCommand = new RelayCommand(ExecuteShowAppListViewCommand);
            ShowSettingsViewCommand = new RelayCommand(ExecShowSettingsViewCommand);

            CurrentChildView = new HomeViewModel();

            //Default view
            LoadCurrentUserData();

        }

        private void ExecShowSettingsViewCommand(object obj)
        {
            CurrentChildView = new SettingsViewModel();
            CurrentWindowTittle = "Settings";
            IconView = IconChar.Tools;
        }

        private void ExecuteShowAppListViewCommand(object obj)
        {
            CurrentChildView = new MyAppListViewModel();
            CurrentWindowTittle = "My List";
            IconView = IconChar.List;
        }

        private void ExecuteShowAddAppViewCommand(object obj)
        {
            CurrentChildView = new AddAppViewModel();
            CurrentWindowTittle = "Add New App";
            IconView = IconChar.AppStore;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            CurrentWindowTittle = "Home";
            IconView = IconChar.Home;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepo.GetByUsername(Thread.CurrentPrincipal.Identity.Name);

            if (user != null)
            {
    
                CurrentUserAccount = new UserAccountModel()
                {
                    UserName = user.UserName,
                    DisplayName = $"{user.Name} {user.LastName}",
                    ProfilePicture = ByteArrayToBitmapImage(user.ProfilePicture)
                };

            }
            else
            {
                MessageBox.Show("Invalid User, not logged in");
                Application.Current.Shutdown();
            }
        }

        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }

    }
}

