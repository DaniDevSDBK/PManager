using PManager.Model;
using PManager.Repositorios;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PManager.ViewModel
{
    /// <summary>
    /// View model utilizado para representar los datos de un usuario en el formulario de registro.
    /// Contiene propiedades que representan los campos del formulario, como el nombre, el correo electrónico, etc.
    /// Este view model se utiliza para recopilar los datos ingresados por el usuario y enviarlos al servicio de API de usuario para el registro.
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {

        private string userName;
        private string email;
        private SecureString password;
        private SecureString confirmPassword;
        private string errorMessage;
        private bool isViewVisible = true;
        private UIRepositable uRepos;

        public string UserName { get => userName; set { userName = value; OnPropertyChanged(nameof(userName)); } }
        public string Email { get => email; set { email = value; OnPropertyChanged(nameof(email)); } }
        public SecureString Password { get => password; set { password = value; OnPropertyChanged(nameof(password)); } }
        public SecureString ConfirmPassword { get => confirmPassword; set { confirmPassword = value; OnPropertyChanged(nameof(confirmPassword)); } }
        public string ErrorMessage { get => errorMessage; set { errorMessage = value; OnPropertyChanged(nameof(errorMessage)); } }
        public bool IsViewVisible { get => isViewVisible; set { isViewVisible = value; OnPropertyChanged(nameof(isViewVisible)); } }
        public UIRepositable URepos { get => uRepos; set => uRepos = value; }
        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            uRepos = new UserRepo();
            RegisterCommand = new RelayCommand(ExecRegisterCommand);
        }

        private async void ExecRegisterCommand(object obj)
        {
            try
            {

                if (UserName.Length>=4 && !String.IsNullOrWhiteSpace(UserName) && !String.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Password.ToString()) && !String.IsNullOrWhiteSpace(ConfirmPassword.ToString()))
                {
                    if (!CheckIfEmailExits() && SecureStringToString(Password).Equals(SecureStringToString(ConfirmPassword)))
                    {

                        var newUser = new UserModel();
                        newUser.UserName = UserName;
                        newUser.Name = UserName;
                        newUser.Email = Email;
                        newUser.ProfilePicture = File.ReadAllBytes("../../../Img/UserProfileDefaultPicture.png");
                        newUser.Password = BCrypt.Net.BCrypt.HashPassword(SecureStringToString(Password), 11);

                        await Task.Run(() =>
                        {

                            if (uRepos.UserExists(newUser.Email))
                            {

                                throw new Exception();

                            }

                        });

                        await Task.Run(() =>
                        {

                            uRepos.Add(newUser);

                        });

                        UserName = "";
                        Email = "";
                        UserName = "";
                        Password.Clear();
                        ConfirmPassword.Clear();
                        ErrorMessage = "Usuario Creado Con Éxito.";
                    }
                    else
                    {
                        ErrorMessage = "El email ya existe. ";
                    }
                }
                else
                {
                    ErrorMessage = "Asegúrese de que la contraseña contiene al menos 6 carácteres";
                }

            }
            catch
            {
                ErrorMessage = "Error al crear usuario.";
            }
            
        }

        private bool CheckIfEmailExits()
        {
            var exists = false;

            try
            {
                exists = String.IsNullOrWhiteSpace(uRepos.GetByEmail(Email).Email)?false:true;
            }
            catch { }

            return exists;
        }
        private string SecureStringToString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

    }
}
