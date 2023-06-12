using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Input;

namespace PManager.ViewModel
{
    using System.Diagnostics;
    using System.Net;
    using System.Windows.Forms;
    using System.Windows.Input;
    using PayPal.Api;
    using PManager.Model;
    using PManager.Repositorios;
    using ICommand = System.Windows.Input.ICommand;

    public class SuscriptionViewModel : BaseViewModel
    {

        private UserContext _currentUserAccount = UserContext.Instance;
        private UserModel _currentUserCopy;
        private UserRepo _userRepo;

        //Commands
        public ICommand UpdateProfilePicture { get; }

        public SuscriptionViewModel()
        {
            UpdateProfilePicture = new RelayCommand(ActualizarSuscrito);
            _userRepo= new UserRepo();
        }

        public async void ActualizarSuscrito(object obj)
        {
            await _userRepo.SuscribeUser(_currentUserAccount.CurrentUser.Email);
        }
    }
}
