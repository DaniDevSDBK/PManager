using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PManager.ViewModel
{
    public class RelayCommand : ICommand
    {
        //ejecuta una acción
        private readonly Action<object> execAction;

        //define si se podrán habilitar o deshabilitar los comandos
        private readonly Predicate<object> canExecAction;

        //No todos los comandos tienen que ser validados para ejecutar la acción, así que creamos un constructor con la acción
        public RelayCommand(Action<object> execAction)
        {
            this.execAction = execAction;
        }

        //para los comandos que necesiten validación
        public RelayCommand(Action<object> execAction, Predicate<object> canExecAction)
        {
            this.execAction = execAction;
            this.canExecAction = canExecAction;
        }

        public event EventHandler? CanExecuteChanged
        {
            //se da si el administrador de comandos detecta que la condición del comando a cambiado
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Devuelve null si no se ha establecido la validación si no devolvemos el método que se va a definir en el modelo de vista
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return canExecAction == null ? true : canExecAction(parameter);
        }

        /// <summary>
        /// Ejecuta la acción
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            execAction(parameter);
        }
    }
}
