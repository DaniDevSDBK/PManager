using FontAwesome.Sharp;
using PManager.Model;
using PManager.Repositorios;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Net.Http.Json;
using PManager.Utils;

namespace PManager.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly HttpClient httpClient;
        private UserAccountModel currentUserAccount;
        private BaseViewModel currentChildView;
        private string _currentWindowTittle;
        private IconChar _iconView;
        private UIRepositable userRepo;
        private UserContext _currentUser = UserContext.Instance;

        public UserContext CurrentUserAccount { get { return _currentUser; } set { _currentUser = value; OnPropertyChanged(nameof(CurrentUserAccount)); } }

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
        public ICommand ShowSuscriptionViewCommand { get; }

        public MainViewModel()
        {
            userRepo = new UserRepo();

            //Initialize
            ShowHomeViewCommand = new RelayCommand(ExecuteShowHomeViewCommand);
            ShowAddAppViewCommand = new RelayCommand(ExecuteShowAddAppViewCommand);
            ShowMyAppListViewCommand = new RelayCommand(ExecuteShowAppListViewCommand);
            ShowSettingsViewCommand = new RelayCommand(ExecShowSettingsViewCommand);
            ShowSuscriptionViewCommand = new RelayCommand(ExecShowSuscriptionViewCommand);

            CurrentChildView = new HomeViewModel();

            //Default view
            LoadCurrentUserData();

        }

        private void ExecShowSuscriptionViewCommand(object obj)
        {
            CurrentChildView = new SuscriptionViewModel();
            CurrentWindowTittle = "Suscription";
            IconView = IconChar.Paypal;
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
            var user = userRepo.GetByEmail(Thread.CurrentPrincipal.Identity.Name);

            if (user != null)
            {

                currentUserAccount = new UserAccountModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    DisplayName = $"{user.Name} {user.LastName}",
                    Email = user.Email,
                    Password = user.Password,
                    ProfilePicture = ByteArrayToBitmapImage(user.ProfilePicture),
                    IsSuscribed = user.Type.Equals(1),
                };

                _currentUser.CurrentUser = currentUserAccount;

                if (_currentUser.CurrentUser.IsSuscribed)
                {
                    _currentUser.CurrentUser.SessionToken = UserApiService.LogIn(user.Email, user.Password).Result ?? String.Empty;
                }

            }
            else
            {
                MessageBox.Show("Invalid User, not logged in");
                Application.Current.Shutdown();
            }
        }

        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            ms.Close();

            return image;
        }
    }
}

