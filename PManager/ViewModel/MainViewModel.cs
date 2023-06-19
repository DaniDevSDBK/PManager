using FontAwesome.Sharp;
using PManager.Model;
using PManager.Repositorios;
using PManager.Utils;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PManager.ViewModel
{
    /// <summary>
    /// View model principal utilizado para la vista principal de la aplicación.
    /// Contiene propiedades y comandos que se utilizan para mostrar y controlar la interfaz de usuario principal.
    /// Este view model es responsable de coordinar la interacción entre otras view models y la vista principal.
    /// </summary>
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

        //RadioButton IsChecked
        #region RadioButtons IsChecked
        private bool _isHomeViewSelected;
        public bool IsHomeViewSelected
        {
            get { return _isHomeViewSelected; }
            set
            {
                _isHomeViewSelected = value;
                OnPropertyChanged(nameof(IsHomeViewSelected));
            }
        }

        private bool _isAddAppViewSelected;
        public bool IsAddAppViewSelected
        {
            get { return _isAddAppViewSelected; }
            set
            {
                _isAddAppViewSelected = value;
                OnPropertyChanged(nameof(IsAddAppViewSelected));
            }
        }

        private bool _isMyAppListViewSelected;
        public bool IsMyAppListViewSelected
        {
            get { return _isMyAppListViewSelected; }
            set
            {
                _isMyAppListViewSelected = value;
                OnPropertyChanged(nameof(IsMyAppListViewSelected));
            }
        }

        private bool _isSettingsViewSelected;
        public bool IsSettingsViewSelected
        {
            get { return _isSettingsViewSelected; }
            set
            {
                _isSettingsViewSelected = value;
                OnPropertyChanged(nameof(IsSettingsViewSelected));
            }
        }

        private bool _isSuscriptionViewSelected;
        public bool IsSuscriptionViewSelected
        {
            get { return _isSuscriptionViewSelected; }
            set
            {
                _isSuscriptionViewSelected = value;
                OnPropertyChanged(nameof(IsSuscriptionViewSelected));
            }
        }
        
        private bool _isHelpViewSelected;
        public bool IsHelpViewSelected
        {
            get { return _isHelpViewSelected; }
            set
            {
                _isHelpViewSelected = value;
                OnPropertyChanged(nameof(IsHelpViewSelected));
            }
        }
        #endregion

        //Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowAddAppViewCommand { get; }
        public ICommand ShowMyAppListViewCommand { get; }
        public ICommand ShowSettingsViewCommand { get; }
        public ICommand ShowLogInViewCommand { get; }
        public ICommand ShowSuscriptionViewCommand { get; }
        public ICommand ShowHelpViewCommand { get; }

        public MainViewModel()
        {
            userRepo = new UserRepo();

            //Initialize
            ShowHomeViewCommand = new RelayCommand(ExecuteShowHomeViewCommand);
            ShowAddAppViewCommand = new RelayCommand(ExecuteShowAddAppViewCommand);
            ShowMyAppListViewCommand = new RelayCommand(ExecuteShowAppListViewCommand);
            ShowSettingsViewCommand = new RelayCommand(ExecShowSettingsViewCommand);
            ShowSuscriptionViewCommand = new RelayCommand(ExecShowSuscriptionViewCommand);
            ShowHelpViewCommand = new RelayCommand(ExecHelpViewCommand);

            CurrentChildView = new HomeViewModel();
            IsHomeViewSelected = true;

            //Default view
            LoadCurrentUserData();

        }

        public void ExecHelpViewCommand(object obj)
        {
            CurrentChildView = new HelpViewModel();
            CurrentWindowTittle = "Help";
            IconView = IconChar.Info;
            IsHelpViewSelected = true;
        }

        public void ExecShowSuscriptionViewCommand(object obj)
        {
            CurrentChildView = new SuscriptionViewModel();
            CurrentWindowTittle = "Suscription";
            IconView = IconChar.Paypal;
            IsSuscriptionViewSelected = true;
        }

        public void ExecShowSettingsViewCommand(object obj)
        {
            CurrentChildView = new SettingsViewModel();
            CurrentWindowTittle = "Settings";
            IconView = IconChar.Tools;
            IsSettingsViewSelected = true;
        }

        public void ExecuteShowAppListViewCommand(object obj)
        {
            CurrentChildView = new MyAppListViewModel();
            CurrentWindowTittle = "My List";
            IconView = IconChar.List;
            IsMyAppListViewSelected = true;
        }

        public void ExecuteShowAddAppViewCommand(object obj)
        {
            CurrentChildView = new AddAppViewModel();
            CurrentWindowTittle = "Add New App";
            IconView = IconChar.AppStore;
            IsAddAppViewSelected = true;
        }

        public void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            CurrentWindowTittle = "Home";
            IconView = IconChar.Home;
            IsHomeViewSelected = true;
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
                MessageBox.Show("*Invalid User, not logged in*");
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

