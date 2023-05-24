using PManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Repositorios
{
    public class UserContext : INotifyPropertyChanged
    {
        private static UserContext instance;
        private static readonly object lockObject = new object();

        private UserAccountModel currentUser;

        private UserContext()
        {
            // Constructor privado para evitar instanciación directa
        }

        public static UserContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new UserContext();
                        }
                    }
                }
                return instance;
            }
        }

        public UserAccountModel CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (currentUser != value)
                {
                    currentUser = value;
                    OnPropertyChanged(nameof(CurrentUser));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
