using PManager.Model;
using PManager.Repositorios;
using PManager.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PManager.ViewModel
{
    public class SuscriptionViewModel : BaseViewModel
    {

        private static UserContext _currentUserAccount = UserContext.Instance;
        private UserRepo _userRepo = new UserRepo();
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        //Commands
        public ICommand Suscribe { get; }

        public SuscriptionViewModel()
        {
            Suscribe = new RelayCommand(Suscribir);
            CheckIfIsSuscriptor();
        }

        public void CheckIfIsSuscriptor()
        {
            if (_currentUserAccount.CurrentUser.IsSuscribed)
            {
                Message = "¡Ya estás suscrito a nuestros servicios!\n¡Gracias por tu colaboración!";
            }
            else
            {
                Message = "¡Suscríbete para obtener ventajas adicionales y apoyar el desarrollo del proyecto!";
            }
        }

        public async void Suscribir(object obj)
        {
            if (!_currentUserAccount.CurrentUser.IsSuscribed)
            {
                _userRepo.SuscribeUser(_currentUserAccount.CurrentUser.Email);
                _currentUserAccount.CurrentUser.IsSuscribed = true;

                await Task.Run(() =>
                {
                    // Copiar la base de datos SQLite
                    string sqliteBackupPath = "../Data/pm.db";
                    CopiarBaseDatosSQLite(sqliteBackupPath);

                });
            }

            CheckIfIsSuscriptor();
        }

        private void CopiarBaseDatosSQLite(string sqliteBackupPath)
        {

            try
            {
                var user = new UserApiModel()
                {
                    Id = _currentUserAccount.CurrentUser.Id,
                    Name = _currentUserAccount.CurrentUser.UserName,
                    Email = _currentUserAccount.CurrentUser.Email,
                    Password = _currentUserAccount.CurrentUser.Password,
                    BackUp = File.ReadAllBytes(sqliteBackupPath)
                };

                UserApiService.InsertSuscribtor(user);
                _currentUserAccount.CurrentUser.SessionToken = UserApiService.LogIn(_currentUserAccount.CurrentUser.Email, _currentUserAccount.CurrentUser.Password).Result ?? String.Empty;
            }
            catch { }
        }

    }
}
