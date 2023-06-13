using Microsoft.Win32;
using PManager.Model;
using PManager.Repositorios;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PManager.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private UserContext _currentUserAccount = UserContext.Instance;
        private UserModel _currentUserCopy;
        private UserRepo _userRepo = new UserRepo();

        public string ErrorLabelMessagge { get; set; }

        public UserContext CurrentUserAccount { get { return _currentUserAccount; } set { this._currentUserAccount = value; } }

        //Commands
        public ICommand UpdateProfilePicture { get; }
        public ICommand UpdateUserData { get; }

        public SettingsViewModel()
        {

            UpdateUserData = new RelayCommand(ExecUserDataUpdate);
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

                CurrentUserAccount.CurrentUser.ProfilePicture = ByteArrayToBitmapImage(imageData);

            }
        }

        private async void ExecUserDataUpdate(object obj)
        {

            try
            {
                this._currentUserCopy = new UserModel()
                {
                    Id = CurrentUserAccount.CurrentUser.Id,
                    Name = CurrentUserAccount.CurrentUser.UserName,
                    UserName = CurrentUserAccount.CurrentUser.UserName,
                    Email = CurrentUserAccount.CurrentUser.Email,
                    ProfilePicture = BitmapImageToByteArray(CurrentUserAccount.CurrentUser.ProfilePicture),
                };

                await Task.Run(() =>
                {
                    _userRepo.Edit(this._currentUserCopy);
                });

                ErrorLabelMessagge = "Perfil Actualizado Correctamente.";
            }
            catch
            {
                ErrorLabelMessagge = "No se pudo actualizar el perfil. Por favor, inténtelo de nuevo más tarde.";
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

        private byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] byteArray;
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
                byteArray = ms.ToArray();
            }
            return byteArray;
        }

    }
}
