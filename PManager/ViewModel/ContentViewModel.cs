using PManager.Repositorios;
using PManager.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PManager.ViewModel
{
    public class ContentViewModel : BaseViewModel
    {
        private UserRepo userRepo = new UserRepo();
        public event Action<ContentViewModel> ItemDeleted;

        private string _userAppName;
        private string _userAppPassword;

        private ICommand _copyCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;

        public ContentViewModel()
        {
            CopyCommand = new RelayCommand(CopyPassword);
            EditCommand = new RelayCommand(EditItem);
            DeleteCommand = new RelayCommand(DeleteItem);
        }

        public string UserAppName
        {
            get { return _userAppName; }
            set { _userAppName = value; OnPropertyChanged("UserAppName"); }
        }

        public string UserAppPassword
        {
            get { return _userAppPassword; }
            set { _userAppPassword = value; OnPropertyChanged("UserAppPassword"); }
        }

        public ICommand CopyCommand
        {
            get { return _copyCommand; }
            set
            {
                _copyCommand = value;
                OnPropertyChanged(nameof(CopyCommand));
            }
        }

        public ICommand EditCommand
        {
            get { return _editCommand; }
            set
            {
                _editCommand = value;
                OnPropertyChanged(nameof(EditCommand));
            }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                _deleteCommand = value;
                OnPropertyChanged(nameof(DeleteCommand));
            }
        }

        private void CopyPassword(object parameter)
        {
            Clipboard.SetText(SecurityService.Decrypt(UserAppPassword));
        }

        private void EditItem(object parameter)
        {
            // Lógica para editar el elemento
        }

        private void DeleteItem(object parameter)
        {
            userRepo.DeleteUserApp(UserAppName, UserAppPassword);
            ItemDeleted?.Invoke(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
