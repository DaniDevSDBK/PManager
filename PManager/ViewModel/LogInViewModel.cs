using PManager.Model;
using PManager.Repositorios;
using PManager.View.NavigationService.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PManager.ViewModel
{
    public class LogInViewModel : BaseViewModel
    {
        private SecureString password;
        private string userName;
        private string errorMessage;
        private bool isViewVisible = true;
        private UIRepositable uRepos;
        private NavigationService nService;

        public string UserName { get => userName; set { userName = value; OnPropertyChanged(nameof(userName)); } }
        public SecureString Password { get => password; set { password = value; OnPropertyChanged(nameof(password));} }
        public string ErrorMessage { get => errorMessage; set { errorMessage = value; OnPropertyChanged(nameof(errorMessage)); } }
        public bool IsViewVisible { get => isViewVisible; set { isViewVisible = value; OnPropertyChanged(nameof(isViewVisible)); } }

        //Comandos para las acciones que va a realizar el usuario
        public ICommand LogInCmd { get;}
        public ICommand RecoverPasswordCmd { get;}
        public ICommand RestorePasswordCmd { get;}
        public ICommand ShowPasswordCmd { get;}
        public ICommand RegisterCommand { get; }

        //
        public LogInViewModel()
        {
            uRepos = new UserRepo();
            nService= new NavigationService();
            LogInCmd = new RelayCommand(ExecLoginCmd, CanExecLogInCmd);
            RegisterCommand = new RelayCommand(ExecRegisterCommand);
            RecoverPasswordCmd = new RelayCommand(p=>ExecRecoverPassword("",""));
        }

        private void ExecRecoverPassword(string userName, string email)
        {
            throw new NotImplementedException();
        }

        private bool CanExecLogInCmd(object obj)
        {
            bool validData;

            if(string.IsNullOrWhiteSpace(UserName) || UserName.Length<4 ||password==null || password.Length < 6)
            {
                validData = false;
            }  
            else
            {
                validData = true;
            }

            return validData;
        }

        private void ExecLoginCmd(object obj)
        {

            var isValidUser = uRepos.AuthenticateUser(new System.Net.NetworkCredential(userName, password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "*Invalid username o password";
            }

        }

        private void ExecRegisterCommand(object obj)
        {
            nService.ShowRegisterView();
        }

    }
}
