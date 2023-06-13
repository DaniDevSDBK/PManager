using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace PManager.Model
{
    public class UserAccountModel : INotifyPropertyChanged
    {
        private BitmapImage _profilePicture;
        private String _username;

        public string UserName
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public int Id { get; set; }

        public String Password { get; set; }

        public String DisplayName { get; set; }

        public String Email { get; set; }

        public String SessionToken { get; set; }

        public bool IsSuscribed { get; set; }

        public BitmapImage ProfilePicture
        {
            get { return _profilePicture; }
            set
            {
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
