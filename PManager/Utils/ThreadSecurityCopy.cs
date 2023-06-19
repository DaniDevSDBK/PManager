using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PManager.Utils
{

    /// <summary>
    /// Clase que representa un hilo de ejecución para realizar una copia de seguridad de seguridad.
    /// </summary>
    public class ThreadSecurityCopy
    {
        private Thread thread;
        private bool isRunning;

        /// <summary>
        /// Inicia el hilo de ejecución para la copia de seguridad de seguridad.
        /// </summary>
        public void StartThreadSecurityCopy()
        {
            isRunning = true;
            thread = new Thread(TaskThread);
            thread.Start();
        }

        /// <summary>
        /// Detiene el hilo de ejecución de la copia de seguridad de seguridad.
        /// </summary>
        public void StopThreadSecurityCopy()
        {
            isRunning = false;
            thread.Join();
        }

        /// <summary>
        /// Método que representa la tarea del hilo de ejecución.
        /// </summary>
        private void TaskThread()
        {
            while (isRunning)
            {
                // Aquí se pueden agregar las acciones necesarias para realizar la copia de seguridad de seguridad
                // Por ejemplo, llamar a un servicio de API para actualizar los datos del usuario
                // UserApiService.UpdateUser();
            }
        }
    }
}
