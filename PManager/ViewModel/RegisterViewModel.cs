using PManager.Model;
using PManager.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PManager.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {

        private string userName;
        private string email;
        private SecureString password;
        private string errorMessage;
        private bool isViewVisible = true;
        private UIRepositable uRepos;

        public string UserName { get => userName; set { userName = value; OnPropertyChanged(nameof(userName)); } }
        public string Email { get => email; set { email = value; OnPropertyChanged(nameof(email)); } }
        public SecureString Password { get => password; set { password = value; OnPropertyChanged(nameof(password)); } }
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
                var newUser = new UserModel();
                newUser.UserName = UserName;
                newUser.Email = Email;
                newUser.Password = Password.ToString();

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

                ErrorMessage = "Usuario Creado Con Éxito.";
            }
            catch
            {
                ErrorMessage = "Error al crear usuario.";
            }
            
        }
    }
}
