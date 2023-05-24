using Microsoft.VisualBasic.ApplicationServices;
using PManager.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PManager.Model
{
    public class UserAccountModel: INotifyPropertyChanged
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
        public String DisplayName { get; set; }
        public String Email { get; set; }

        public String SessionToken { get; set; }

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
