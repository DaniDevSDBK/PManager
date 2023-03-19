using Microsoft.Win32;
using PManager.Model;
using PManager.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PManager.ViewModel
{
    public class SettingsViewModel:BaseViewModel
    {

        private UserAccountModel _currentUserAccountCopy;
        private UserModel _currentUserCopy;
        private UserRepo _userRepo = new UserRepo();

        public string ErrorLabelMessagge { get; set; }

        public UserAccountModel CurrentUserAccountCopy { get { return _currentUserAccountCopy; } set { this._currentUserAccountCopy = value;} }

        //Commands
        public ICommand UpdateProfilePicture { get; }
        public ICommand UpdateUserData { get; }

        public SettingsViewModel()
        {

            LoadParentInfo();

            UpdateProfilePicture = new RelayCommand(ExecUpdateProfilePicture);
            UpdateUserData = new RelayCommand(ExecUserDataUpdate);

        }
        public void LoadParentInfo()
        {

            var mainWindow = (MainWindow)System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            var mainWindowViewModel = (MainViewModel)mainWindow.DataContext;

            _currentUserAccountCopy = new UserAccountModel()
            {
                UserName = mainWindowViewModel.CurrentUserAccount.UserName,
                Email= mainWindowViewModel.CurrentUserAccount.Email,
                ProfilePicture = mainWindowViewModel.CurrentUserAccount.ProfilePicture,

            };

        }

        private void ExecUpdateProfilePicture(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png";

            // Abre el diálogo y verifica que el usuario ha seleccionado un archivo
            if (openFileDialog.ShowDialog() == true)
            {
                // Lee la imagen seleccionada en un array de bytes
                byte[] imageData = File.ReadAllBytes(openFileDialog.FileName);

                CurrentUserAccountCopy.ProfilePicture = ByteArrayToBitmapImage(imageData);

                this._currentUserCopy = new UserModel()
                {
                    Id = "04655612K",
                    Name = CurrentUserAccountCopy.UserName,
                    UserName = CurrentUserAccountCopy.UserName,
                    Email = CurrentUserAccountCopy.Email,
                    ProfilePicture = imageData,

                };
            }
        }

        private void ExecUserDataUpdate(object obj)
        {

            _userRepo.Edit(this._currentUserCopy);  
            ErrorLabelMessagge = "Todo Bien";

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
