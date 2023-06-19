using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PManager.ViewModel
{
    /// <summary>
    /// View model base que proporciona funcionalidades comunes a otros view models.
    /// Contiene propiedades y métodos comunes que se pueden heredar y reutilizar en los view models derivados.
    /// Este view model proporciona una base para la implementación consistente de la lógica compartida en varios view models.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
